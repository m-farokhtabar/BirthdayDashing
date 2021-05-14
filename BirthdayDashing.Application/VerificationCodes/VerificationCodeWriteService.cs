using BirthdayDashing.Application.Dtos.Emails.Input;
using BirthdayDashing.Application.Dtos.VerficationCodes.Input;
using BirthdayDashing.Application.Emails;
using BirthdayDashing.Application.Users;
using BirthdayDashing.Domain.SeedWork;
using BirthdayDashing.Domain.VerificationCodes;
using Common.Exception;
using System.Threading.Tasks;
using static Common.Exception.Messages;

namespace BirthdayDashing.Application.VerificationCodes
{
    public class VerificationCodeWriteService : IVerificationCodeWriteService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IVerificationCodeRepository Repository;
        private readonly IEmailService EmailService;
        private readonly IUserReadService UserReadService;

        public VerificationCodeWriteService(IUnitOfWork unitOfWork, IVerificationCodeRepository repository, IEmailService emailService, IUserReadService userReadService)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
            EmailService = emailService;
            UserReadService = userReadService;
        }
        public async Task NewCodeForConfirmEmailAsync(ReSendConfirmEmailDto confirmEmail)
        {
            var userInfo = await UserReadService.GetEssentialDataAsync(confirmEmail.Id);
            if (userInfo is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, nameof(confirmEmail.Id));
            if (userInfo.Roles == null || userInfo.Roles.Count == 0)
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "Role");
            if (userInfo.IsApproved)
                throw new ManualException(DATA_IS_ALREADY_APPROVED.Replace("{0}", "User"), ExceptionType.Conflict, nameof(userInfo.IsApproved));

            VerificationCode Entity = new(userInfo.Id, EmailService.GetConfirmEmailExpireTimeInMinute());
            try
            {
                await Repository.AddAsync(Entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
            await EmailService.SendConfirmEmail(new SendConfirmEmailDto() { UserId = userInfo.Id, Email = userInfo.Email, Token = Entity.Token });
        }
        public async Task NewCodeForForgotPasswordAsync(ForgotPasswordDto forgotPasswordEmail)
        {
            var userInfo = await UserReadService.GetEssentialDataByEmailAsync(forgotPasswordEmail.Email);
            if (userInfo is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, nameof(forgotPasswordEmail.Email));
            if (userInfo.Roles == null || userInfo.Roles.Count == 0)
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "Role");
            if (!userInfo.IsApproved)
                throw new ManualException(USER_IS_NOT_APPROVED, ExceptionType.UnAuthorized, nameof(userInfo.IsApproved));

            VerificationCode Entity = new(userInfo.Id, EmailService.GetConfirmEmailExpireTimeInMinute(), VerificationType.ForgotPasswordByEmail);
            try
            {
                await Repository.AddAsync(Entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
            await EmailService.SendForgotPasswordEmail(new SendForgotPasswordEmailDto() { UserId = userInfo.Id, Email = userInfo.Email, Token = Entity.Token });
        }
    }
}
