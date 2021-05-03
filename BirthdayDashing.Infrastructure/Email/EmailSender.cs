using BirthdayDashing.Application.Email;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System;

namespace BirthdayDashing.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        public IEmailSetting Setting { get; }
        public IHostAddresses HostAddresses { get; }

        public EmailSender(IEmailSetting setting, IHostAddresses hostAddresses)
        {
            Setting = setting;
            HostAddresses = hostAddresses;
        }
        public async Task SendEmailAsync(string recepient, string subject, string body, bool isHtml = true)
        {
            MailMessage mailMessage = new()
            {
                From = new MailAddress(Setting.AppEmail),
                Subject = subject,
                IsBodyHtml = isHtml,
                Body = body,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
            };
            mailMessage.To.Add(recepient);

            using (var client = new SmtpClient(Setting.Host))
            {
                client.Credentials = new NetworkCredential(Setting.UserName, Setting.Password);
                client.Port = Setting.Port;
                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch(Exception ex)
                {
                }
            }
        }
    }
}
