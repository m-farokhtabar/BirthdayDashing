using BirthdayDashing.Application.Configuration.Data;
using BirthdayDashing.Application.Configuration.Email;
using BirthdayDashing.Domain.SeedWork;
using BirthdayDashing.Domain.Users;
using BirthdayDashing.Domain.VerificationCodes;
using BirthdayDashing.Infrastructure.Data.Read;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.Email;
using BirthdayDashing.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BirthdayDashing.Infrastructure.StartupConfig
{
    public static class InfrastructureServiceExtension
    {
        public static void ConfigureDataAccess(this IServiceCollection services,string ConnectionString)
        {
            //Write
            services.AddScoped<DbContext>(x=> new DbContext(ConnectionString));
            services.AddScoped<IDbContext, DbContext>(x=>x.GetRequiredService<DbContext>());
            services.AddScoped<IDbSet, DbContext>(x => x.GetRequiredService<DbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IManageDbExceptionUniqueAndKeyFields, ManageDbExceptionUniqueAndKeyFields>();

            //Read            
            services.AddScoped<IReadDbSet, ReadDbSet>(x => new ReadDbSet(ConnectionString));
        }
        public static void ConfigureRepositories(this IServiceCollection services)
        {            
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IVerificationCodeRepository, VerificationCodeRepository>();
        }
        public static void ConfigureEmail(this IServiceCollection services)
        {            
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>();
        }
    }
}
