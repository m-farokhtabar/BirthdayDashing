using System.Threading.Tasks;

namespace BirthdayDashing.Application.Email
{
    public interface IEmailTemplateProvider
    {
        Task<string> Get(string TemplateName);
    }
}
