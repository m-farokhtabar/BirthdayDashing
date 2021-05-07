using System.Threading.Tasks;

namespace BirthdayDashing.Application.Configuration.Email
{
    public interface IEmailSender
    {
        IEmailSetting Setting { get; }
        IHostAddresses HostAddresses { get; }
        Task SendEmailAsync(string recepients, string subject, string body, bool isHtml = true);
    }
}
