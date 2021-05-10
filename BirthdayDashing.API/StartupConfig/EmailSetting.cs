using BirthdayDashing.Application.Configuration.Email;

namespace BirthdayDashing.API.StartupConfig
{
    public class EmailSetting : IEmailSetting
    {
        public string[] CampaignTemplateSetting { get; set; }
        public string[] CommentUpdateTemplateSetting { get; set; }
        public string[] DashingUpdateTempalateSetting { get; set; }
        public string[] DonationTemplateSetting { get; set; }
        public string[] EmailCampaignTemplateSetting { get; set; }
        public string[] ForgotPasswordTemplateSetting { get; set; }
        public string[] HappyBirthdayTemplateSetting { get; set; }
        public string[] HappyBirthdayToTemplateSetting { get; set; }
        public string[] InvitationTemplateSetting { get; set; }
        public string[] NotificationTemplateSetting { get; set; }
        public string[] NewDonationTemplateSetting { get; set; }
        public string[] PasswordChangeTemplateSetting { get; set; }
        public string[] ReferralEmailTemplateSetting { get; set; }
        public string[] ResetPasswordTemplateSetting { get; set; }
        public string[] ThankYouTemplateSetting { get; set; }
        public string[] VerifyCodeTemplateSetting { get; set; }
        public string[] WelcomeTemplateSetting { get; set; }
        public string[] WelcomeNewDasherTemplateSetting { get; set; }
        public string TemplatePhysicalPath { get; set; }
        public int TempateCacheExpireTimeInHour { get; set; }

        public string AppEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int ConfirmEmailExpireTimeInMinute { get; set; }

    }
}
