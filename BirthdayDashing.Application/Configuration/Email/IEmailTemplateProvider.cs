using System.Threading.Tasks;

namespace BirthdayDashing.Application.Configuration.Email
{
    public interface IEmailTemplateProvider
    {
        Task<string> Get(string TemplateName);
    }
}
