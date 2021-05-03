using BirthdayDashing.Application.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace BirthdayDashing.API.Helper
{
    public class HostAddresses : IHostAddresses
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly IWebHostEnvironment Host;

        public HostAddresses(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment host)
        {
            HttpContextAccessor = httpContextAccessor;
            Host = host;
        }

        public string BaseUrl => HttpContextAccessor.HttpContext.Request.Scheme + "://" + HttpContextAccessor.HttpContext.Request.Host.Value;

        public string BasePhysicalAddress => Host.WebRootPath;
    }
}
