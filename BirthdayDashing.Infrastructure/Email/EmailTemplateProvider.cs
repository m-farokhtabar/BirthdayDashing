using BirthdayDashing.Application.Configuration.Email;
using System.IO;
using System.Threading.Tasks;

namespace BirthdayDashing.Infrastructure.Email
{
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        private readonly IEmailSetting Setting;
        private readonly IHostAddresses HostAddresses;
        public EmailTemplateProvider(IEmailSetting setting, IHostAddresses hostAddresses)
        {
            Setting = setting;
            HostAddresses = hostAddresses;
        }

        public async Task<string> Get(string TemplateName)
        {
            try
            {
                using StreamReader SourceReader = File.OpenText(Path.Combine(HostAddresses.BasePhysicalAddress, Setting.TemplatePhysicalPath, TemplateName));
                return await SourceReader.ReadToEndAsync();
            }
            catch
            {

            }
            return null;
        }
    }
}
