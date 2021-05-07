namespace BirthdayDashing.Application.Configuration.Email
{
    public interface IEmailSetting
    {
        string[] CampaignTemplateSetting { get; set; }
        string[] CommentUpdateTemplateSetting { get; set; }
        string[] DashingUpdateTempalateSetting { get; set; }
        string[] DonationTemplateSetting { get; set; }
        string[] EmailCampaignTemplateSetting { get; set; }
        string[] ForgotPasswordTemplateSetting { get; set; }
        string[] HappyBirthdayTemplateSetting { get; set; }
        string[] HappyBirthdayToTemplateSetting { get; set; }
        string[] InvitationTemplateSetting { get; set; }
        string[] NotificationTemplateSetting { get; set; }
        string[] NewDonationTemplateSetting { get; set; }
        string[] PasswordChangeTemplateSetting { get; set; }
        string[] ReferralEmailTemplateSetting { get; set; }
        string[] ResetPasswordTemplateSetting { get; set; }
        string[] ThankYouTemplateSetting { get; set; }
        string[] VerifyCodeTemplateSetting { get; set; }
        string[] WelcomeTemplateSetting { get; set; }
        string[] WelcomeNewDasherTemplateSetting { get; set; }
        string TemplatePhysicalPath { get; set; }
        int TempateCacheExpireInHour { get; set; }

        string AppEmail { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        int ConfirmEmailExpireTimeInMinute { get; set; }
    }
}
