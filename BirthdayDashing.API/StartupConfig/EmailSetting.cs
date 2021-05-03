using BirthdayDashing.Application.Email;

namespace BirthdayDashing.API.StartupConfig
{
    public class EmailSetting : IEmailSetting
    {
        public string[] CampaignTemplateName { get; set; }
        public string[] CommentUpdateTemplateName { get; set; }
        public string[] DashingUpdateTempalateName { get; set; }
        public string[] DonationTemplateName { get; set; }
        public string[] EmailCampaignTemplateName { get; set; }
        public string[] ForgotPasswordTemplateName { get; set; }
        public string[] HappyBirthdayTemplateName { get; set; }
        public string[] HappyBirthdayToTemplateName { get; set; }
        public string[] InvitationTemplateName { get; set; }
        public string[] NotificationTemplateName { get; set; }
        public string[] NewDonationTemplateName { get; set; }
        public string[] PasswordChangeTemplateName { get; set; }
        public string[] ReferralEmailTemplateName { get; set; }
        public string[] ResetPasswordTemplateName { get; set; }
        public string[] ThankYouTemplateName { get; set; }
        public string[] VerifyCodeTemplateName { get; set; }
        public string[] WelcomeTemplateName { get; set; }
        public string[] WelcomeNewDasherTemplateName { get; set; }
        public string TemplatePhysicalPath { get; set; }
        public int TempateCacheExpireInHour { get; set; }

        public string AppEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string ConfirmPageUrl { get; set; }
        public int ConfirmEmailExpireTimeInMinute { get; set; }
    }
}
