using System.Threading.Tasks;

namespace BirthdayDashing.Application.Email
{
    public interface IEmailSender
    {
        IEmailSetting Setting { get; }
        IHostAddresses HostAddresses { get; }
        Task SendEmailAsync(string recepients, string subject, string body, bool isHtml = true);
    }
}
