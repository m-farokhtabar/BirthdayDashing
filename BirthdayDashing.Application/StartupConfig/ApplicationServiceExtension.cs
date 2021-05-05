using BirthdayDashing.Application.Requests.Read.Roles;
using BirthdayDashing.Application.Requests.Read.Users;
using BirthdayDashing.Application.Requests.Write.Users;
using Microsoft.Extensions.DependencyInjection;

namespace BirthdayDashing.Application.StartupConfig
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            //Write
            services.AddTransient<IUserWriteService, UserWriteService>();            

            //Read
            services.AddTransient<IUserReadService, UserReadService>();
            services.AddTransient<IRoleReadService, RoleReadService>();
        }        
    }
}
