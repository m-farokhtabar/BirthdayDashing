using BirthdayDashing.Application.Dtos.Emails.Input;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Emails
{
    public interface IEmailService
    {
        int GetConfirmEmailExpireTimeInMinute();
        Task SendConfirmEmail(SendConfirmEmailDto confirmEmail);
        Task SendForgotPasswordEmail(SendForgotPasswordEmailDto forgetPasswordEmail);
        Task SendWelcomeEmail(SendWelcomeEmailDto WelcomeEmail);
    }
}