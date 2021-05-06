using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Requests.Emails;
using BirthdayDashing.Application.Requests.Read.Roles;
using BirthdayDashing.Domain;
using BirthdayDashing.Domain.Data;
using BirthdayDashing.Domain.Repository;
using Common.Exception;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Common.Exception.Messages;

namespace BirthdayDashing.Application.Requests.Write.Users
{
    public class UserWriteService : IUserWriteService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IUserRepository Repository;
        private readonly IVerificationCodeRepository VerificationCodeRepository;
        private readonly IEmailService EmailService;
        private readonly IRoleReadService RoleReadService;

        public UserWriteService(IUnitOfWork unitOfWork, IUserRepository repository, IVerificationCodeRepository verificationCodeRepository, IEmailService emailService, IRoleReadService roleReadService)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
            VerificationCodeRepository = verificationCodeRepository;
            EmailService = emailService;
            RoleReadService = roleReadService;
        }
        public async Task AddAsync(AddUserDto user)
        {
            //Get Default Role for Users
            var UserRoleId = await RoleReadService.GetIdByNameAsync(Role.User);
            if (UserRoleId is null || UserRoleId.HasValue == false)
                throw new ManualException(THERE_ARE_ANY_DATA_TO_ASSIGN.Replace("{0}", "Roles"), ExceptionType.NotFound);
            User entity = new(user.Email, user.Password, user.PostalCode, user.Birthday, UserRoleId.Value, user.FirstName, user.LastName);
            VerificationCode VerificationCodeEntity = new(entity.Id, EmailService.GetConfirmEmailExpireTimeInMinute());
            try
            {
                await Repository.AddAsync(entity);
                await VerificationCodeRepository.AddAsync(VerificationCodeEntity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }            
            await EmailService.SendConfirmEmail(new SendConfirmEmailDto() { UserId = entity.Id, Email = entity.Email, Token = VerificationCodeEntity.Token });
        }

        public async Task ConfirmByEmailAsync(ConfirmUserDto confirmUser)
        {
            User entity = await Repository.GetAsync(confirmUser.UserId);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, new string[] { nameof(confirmUser.UserId) });
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
                    throw new ManualException(DATA_IS_NOT_VALID.Replace("{0}", "Code"), ExceptionType.InValid, new string[] { nameof(confirmUser.Token) });
                try
                {
                    await Repository.UpdateIsApprovedAsync(entity);
                    UnitOfWork.SaveChanges();
                }
                catch
                {
                    UnitOfWork.RollBack();
                    throw;
                }
            }
        }
        public async Task UpdateAsync(Guid id, UpdateUserDto user)
        {
            User entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, new string[] { nameof(id) });
            entity.Update(user.PostalCode, user.Birthday, user.FirstName, user.LastName, user.PhoneNumber, user.ImageUrl);
            await Repository.UpdateAsync(entity);
            UnitOfWork.SaveChanges();
        }
        public async Task ChangePasswordAsync(Guid id, ChangePasswordDto password)
        {
            User entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, new string[] { nameof(id) });
            entity.UpdatePassword(password.NewPassword, password.OldPassword);
            try
            {
                await Repository.UpdatePasswordAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
        }
    }
}
