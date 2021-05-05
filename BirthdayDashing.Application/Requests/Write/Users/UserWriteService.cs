using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Email;
using BirthdayDashing.Application.Requests.Read.Roles;
using BirthdayDashing.Domain;
using BirthdayDashing.Domain.Data;
using BirthdayDashing.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Requests.Write.Users
{
    public class UserWriteService : IUserWriteService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IUserRepository Repository;
        private readonly IVerificationCodeRepository VerificationCodeRepository;
        private readonly IEmailSender EmailSender;
        private readonly IEmailTemplateProvider TemplateProvider;
        private readonly IRoleReadService RoleReadService;

        public UserWriteService(IUnitOfWork unitOfWork, IUserRepository repository, IVerificationCodeRepository verificationCodeRepository, IEmailSender emailSender, IEmailTemplateProvider templateProvider, IRoleReadService roleReadService)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
            VerificationCodeRepository = verificationCodeRepository;
            EmailSender = emailSender;
            TemplateProvider = templateProvider;
            RoleReadService = roleReadService;
        }
        public async Task AddAsync(AddUserDto user)
        {
            //Get Default Role for Users
            var UserRoleId = await RoleReadService.GetIdByNameAsync(Role.User);
            if (UserRoleId is null || UserRoleId.HasValue == false)
                throw new Exception("There are not any roles to assign");
            User entity = new(user.Email, user.Password, user.PostalCode, user.Birthday, UserRoleId.Value);
            await Repository.AddAsync(entity);
            VerificationCode VerificationCodeEntity = new(entity.Id, EmailSender.Setting.ConfirmEmailExpireTimeInMinute);
            await VerificationCodeRepository.AddAsync(VerificationCodeEntity);
            UnitOfWork.SaveChanges();
            await SendConfirmEmail(entity, VerificationCodeEntity);
        }

        public async Task ConfirmByEmailAsync(ConfirmUserDto confirmUser)
        {
            User entity = await Repository.GetAsync(confirmUser.UserId);
            if (entity is null)
                throw new Exception("User is not found");
            if (!entity.IsApproved)
            {
                List<VerificationCode> VerificationCodeEntities = await VerificationCodeRepository.GetAsync(confirmUser.UserId, confirmUser.Token);
                foreach (var item in VerificationCodeEntities)
                {
                    if (item.ExpireDate.CompareTo(DateTime.Now) > 0 && item.Type == VerificationType.Email)
                    {
                        entity.Approved();
                        break;
                    }
                }
                if (!entity.IsApproved)
                    throw new Exception("Code is not valid");
                await Repository.UpdateIsApprovedAsync(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public async Task UpdateAsync(Guid id, UpdateUserDto user)
        {
            User entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new Exception("User is not found");
            entity.Update(user.PostalCode, user.Birthday, user.FirstName, user.LastName, user.PhoneNumber, user.ImageUrl);
            await Repository.UpdateAsync(entity);
            UnitOfWork.SaveChanges();
        }
        public async Task ChangePasswordAsync(Guid id, ChangePasswordDto password)
        {
            User entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new Exception("User is not found");
            entity.UpdatePassword(password.NewPassword,password.OldPassword);
            await Repository.UpdatePasswordAsync(entity);
            UnitOfWork.SaveChanges();
        }

        private async Task SendConfirmEmail(User entity, VerificationCode VerificationCodeEntity)
        {
            string Body = await TemplateProvider.Get(EmailSender.Setting.VerifyCodeTemplateName[0]);
            string Subject = EmailSender.Setting.VerifyCodeTemplateName[1];
            string ConfirmPageUrl = EmailSender.HostAddresses.BaseUrl + EmailSender.Setting.ConfirmPageUrl.Replace("{0}", entity.Id.ToString()).Replace("{1}", entity.Email).Replace("{2}", VerificationCodeEntity.Token);
            Body = Body.Replace("{0}", VerificationCodeEntity.Token).Replace("{1}", ConfirmPageUrl);

            await EmailSender.SendEmailAsync(entity.Email, Subject, Body);
        }
    }
}
