using BirthdayDashing.Application.Dtos.VerficationCodes.Input;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.VerificationCodes
{
    public interface IVerificationCodeWriteService
    {
        Task NewCodeForConfirmEmailAsync(NewConfirmEmailDto NewConfirmEmail);
        Task NewCodeForConfirmEmailAsync(ReSendConfirmEmailDto confirmEmail);
        Task NewCodeForForgotPasswordAsync(ForgotPasswordDto forgotPasswordEmail);
    }
}