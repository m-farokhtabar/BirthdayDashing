using BirthdayDashing.Application.Configuration.Email;
using BirthdayDashing.Application.Dtos.Emails.Input;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Emails
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender EmailSender;
        private readonly IEmailTemplateProvider TemplateProvider;
        public EmailService(IEmailSender emailSender, IEmailTemplateProvider templateProvider)
        {
            EmailSender = emailSender;
            TemplateProvider = templateProvider;
        }
        public int GetConfirmEmailExpireTimeInMinute()
        {
            return EmailSender.Setting.ConfirmEmailExpireTimeInMinute;
        }
        public async Task SendConfirmEmail(SendConfirmEmailDto confirmEmail)
        {
            var Setting = EmailSender.Setting.VerifyCodeTemplateSetting;
            string Body = await TemplateProvider.Get(Setting[0]);
            string Subject = Setting[1];
            string ConfirmPageUrl = EmailSender.HostAddresses.BaseUrl + Setting[2].Replace("{0}", confirmEmail.UserId.ToString()).Replace("{1}", confirmEmail.Token);
            Body = Body.Replace("{0}", confirmEmail.Token).Replace("{1}", ConfirmPageUrl);

            await EmailSender.SendEmailAsync(confirmEmail.Email, Subject, Body);
        }
        public async Task SendForgotPasswordEmail(SendForgotPasswordEmailDto forgetPasswordEmail)
        {
            var Setting = EmailSender.Setting.ForgotPasswordTemplateSetting;
            string Body = await TemplateProvider.Get(Setting[0]);
            string Subject = Setting[1];
            string ResetPageUrl = EmailSender.HostAddresses.BaseUrl + Setting[2].Replace("{0}", forgetPasswordEmail.UserId.ToString()).Replace("{1}", forgetPasswordEmail.Token);
            Body = Body.Replace("{0}", forgetPasswordEmail.Email).Replace("{1}", ResetPageUrl);

            await EmailSender.SendEmailAsync(forgetPasswordEmail.Email, Subject, Body);
        }
        public async Task SendWelcomeEmail(SendWelcomeEmailDto WelcomeEmail)
        {
            var Setting = EmailSender.Setting.WelcomeTemplateSetting;
            string Body = await TemplateProvider.Get(Setting[0]);
            string Subject = Setting[1];
            string LoginPageUrl = EmailSender.HostAddresses.BaseUrl + Setting[2];
            Body = Body.Replace("{0}", WelcomeEmail.Email).Replace("{1}", LoginPageUrl);

            await EmailSender.SendEmailAsync(WelcomeEmail.Email, Subject, Body);
        }
    }
}
