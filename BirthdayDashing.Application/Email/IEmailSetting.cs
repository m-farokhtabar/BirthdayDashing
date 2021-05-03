namespace BirthdayDashing.Application.Email
{
    public interface IEmailSetting
    {
        string[] CampaignTemplateName { get; set; }
        string[] CommentUpdateTemplateName { get; set; }
        string[] DashingUpdateTempalateName { get; set; }
        string[] DonationTemplateName { get; set; }
        string[] EmailCampaignTemplateName { get; set; }
        string[] ForgotPasswordTemplateName { get; set; }
        string[] HappyBirthdayTemplateName { get; set; }
        string[] HappyBirthdayToTemplateName { get; set; }
        string[] InvitationTemplateName { get; set; }
        string[] NotificationTemplateName { get; set; }
        string[] NewDonationTemplateName { get; set; }
        string[] PasswordChangeTemplateName { get; set; }
        string[] ReferralEmailTemplateName { get; set; }
        string[] ResetPasswordTemplateName { get; set; }
        string[] ThankYouTemplateName { get; set; }
        string[] VerifyCodeTemplateName { get; set; }
        string[] WelcomeTemplateName { get; set; }
        string[] WelcomeNewDasherTemplateName { get; set; }
        string TemplatePhysicalPath { get; set; }
        int TempateCacheExpireInHour { get; set; }

        string AppEmail { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        string ConfirmPageUrl { get; set; }
        int ConfirmEmailExpireTimeInMinute { get; set; }
    }
}
