using BirthdayDashing.Application.Emails;
using BirthdayDashing.Application.Roles;
using BirthdayDashing.Application.Users;
using BirthdayDashing.Application.VerificationCodes;
using Microsoft.Extensions.DependencyInjection;

namespace BirthdayDashing.Application.Configuration.StartupConfig
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            //User
            services.AddTransient<IUserWriteService, UserWriteService>();
            services.AddTransient<IUserReadService, UserReadService>();

            //Role
            services.AddTransient<IRoleReadService, RoleReadService>();

            //VerificationCode
            services.AddTransient<IVerificationCodeWriteService, VerificationCodeWriteService>();

            //Email
            services.AddTransient<IEmailService, EmailService>();
        }        
    }
}
