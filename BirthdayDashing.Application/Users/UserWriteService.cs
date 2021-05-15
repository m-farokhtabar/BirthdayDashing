using BirthdayDashing.Application.Configuration.Setting;
using BirthdayDashing.Application.Dtos.Emails.Input;
using BirthdayDashing.Application.Dtos.Roles.Output;
using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Dtos.Users.Output;
using BirthdayDashing.Application.Dtos.VerficationCodes.Output;
using BirthdayDashing.Application.Emails;
using BirthdayDashing.Application.Roles;
using BirthdayDashing.Application.VerificationCodes;
using BirthdayDashing.Domain.Roles;
using BirthdayDashing.Domain.SeedWork;
using BirthdayDashing.Domain.Users;
using BirthdayDashing.Domain.VerificationCodes;
using Common.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Exception.Messages;

namespace BirthdayDashing.Application.Users
{
    public class UserWriteService : IUserWriteService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IUserRepository Repository;
        private readonly IVerificationCodeRepository VerificationCodeRepository;
        private readonly IVerificationCodeReadService VerificationCodeReadService;
        private readonly IEmailService EmailService;
        private readonly IRoleReadService RoleReadService;
        private readonly ISettings AppSettings;

        public UserWriteService(IUnitOfWork unitOfWork, IUserRepository repository, IVerificationCodeRepository verificationCodeRepository, IVerificationCodeReadService verificationCodeReadService, IEmailService emailService, IRoleReadService roleReadService, ISettings appSettings)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
            VerificationCodeRepository = verificationCodeRepository;
            VerificationCodeReadService = verificationCodeReadService;            
            EmailService = emailService;
            RoleReadService = roleReadService;
            AppSettings = appSettings;
        }
        public async Task AddAsync(AddUserDto user)
        {
            //Get Default Role for Users
            var UserRoleId = await RoleReadService.GetIdByNameAsync(Role.User);
            if (UserRoleId is null || UserRoleId.HasValue == false)
                throw new ManualException(THERE_ARE_ANY_DATA_TO_ASSIGN.Replace("{0}", "Roles"), ExceptionType.NotFound, "Role");
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
        public async Task ConfirmByEmailAsync(ConfirmUserByEmailDto confirmUser)
        {
            User entity = await Repository.GetAsync(confirmUser.UserId);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, nameof(confirmUser.UserId));
            if (entity.UserRoles is null || entity.UserRoles.Count == 0)
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "Role");
            if (entity.IsApproved)
                throw new ManualException(DATA_IS_ALREADY_APPROVED.Replace("{0}", "User"), ExceptionType.Conflict, nameof(entity.IsApproved));

            List<VerificationCodeDataForVerifyDto> VCodes = await VerificationCodeReadService.GetDataForVerifyAsync(confirmUser.UserId, confirmUser.Token);
            foreach (var item in VCodes)
            {
                if (item.ExpireDate.CompareTo(DateTime.Now) > 0 && item.Type == VerificationTypeDto.ConfirmUserByEmail)
                {
                    entity.Approved();
                    break;
                }
            }
            if (!entity.IsApproved)
                throw new ManualException(DATA_IS_NOT_VALID.Replace("{0}", "Code"), ExceptionType.InValid, nameof(confirmUser.Token));
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
            await EmailService.SendWelcomeEmail(new SendWelcomeEmailDto() { Email = entity.Email });
        }
        public async Task UpdateAsync(Guid id, UpdateUserDto user)
        {
            User entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, nameof(id));
            entity.Update(user.PostalCode, user.Birthday, user.FirstName, user.LastName, user.PhoneNumber, user.ImageUrl);
            try
            {
                await Repository.UpdateAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
        }
        public async Task ChangePasswordAsync(Guid id, ChangePasswordDto password)
        {
            User entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, nameof(id));
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
        public async Task ResetPasswordAsync(Guid id, ResetPasswordDto password)
        {
            User entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, nameof(id));
            if (entity.UserRoles is null || entity.UserRoles.Count == 0)
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "Role");
            if (!entity.IsApproved)
                throw new ManualException(USER_IS_NOT_APPROVED, ExceptionType.UnAuthorized, nameof(entity.IsApproved));

            bool TokenIsAcceptable = false;
            List<VerificationCodeDataForVerifyDto> VCodes = await VerificationCodeReadService.GetDataForVerifyAsync(id, password.Token);
            foreach (var item in VCodes)
            {
                if (item.ExpireDate.CompareTo(DateTime.Now) > 0 && item.Type == VerificationTypeDto.ForgotPasswordByEmail)
                {
                    entity.ResetPassword(password.NewPassword);
                    TokenIsAcceptable = true;
                    break;
                }
            }
            if (!TokenIsAcceptable)
                throw new ManualException(DATA_IS_NOT_VALID.Replace("{0}", "Code"), ExceptionType.InValid, nameof(password.Token));
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
        public async Task<UserLoginDto> Login(LoginDto login)
        {
            User entity = await Repository.GetByEmailAsync(login.Email);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, nameof(login.Email));

            if (entity.UserRoles is null || entity.UserRoles.Count == 0)
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "Role");

            if (!entity.IsApproved)
                throw new ManualException(USER_IS_NOT_APPROVED, ExceptionType.UnAuthorized, nameof(entity.IsApproved));

            if (entity.LockOutThreshold >= AppSettings.MaxLockOutThreshold)
                throw new ManualException(YOU_HAVE_ENTERED_THE_WRONG_PASSWORD_TOO_MANY_TIMES, ExceptionType.UnAuthorized, "LockOutThreshold");

            if (!Common.Security.VerifyPassword(login.Password, entity.Password))
            {
                entity.IncreaseLockOutThreshold();
                try
                {
                    await Repository.UpdateLockOutThresholdAsync(entity);
                    UnitOfWork.SaveChanges();
                }
                catch
                {
                    UnitOfWork.RollBack();
                    throw;
                }
                throw new ManualException(DATA_IS_INCORRECT.Replace("{0}", nameof(login.Password)), ExceptionType.NotFound, nameof(login.Password));
            }
            var Roles = await RoleReadService.GetAllAsync();
            if (Roles is null || Roles.Count == 0)
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "Role");

            entity.UpdateLastLoginDate();
            try
            {
                await Repository.UpdateLastLoginDateAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
            return new UserLoginDto()
            {
                Id = entity.Id,
                Birthday = entity.Birthday,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                ImageUrl = entity.ImageUrl,
                PhoneNumber = entity.PhoneNumber,
                PostalCode = entity.PostalCode,
                RolesName = Roles.Where(x => entity.UserRoles.Any(y => y.RoleId == x.Id)).Select(x => new RoleNameDto() { Name = x.Name }).ToList()
            };
        }
    }
}
