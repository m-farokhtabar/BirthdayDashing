using BirthdayDashing.Application.Data;
using BirthdayDashing.Application.Email;
using BirthdayDashing.Domain.Data;
using BirthdayDashing.Domain.Repository;
using BirthdayDashing.Infrastructure.Data.Read;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.Email;
using BirthdayDashing.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BirthdayDashing.Infrastructure.StartupConfig
{
    public static class InfrastructureServiceExtention
    {
        public static void ConfigureDataAccess(this IServiceCollection services,string ConnectionString)
        {
            //Write
            services.AddScoped<DbContext>(x=> new DbContext(ConnectionString));
            services.AddScoped<IDbContext, DbContext>(x=>x.GetRequiredService<DbContext>());
            services.AddScoped<IDbSet, DbContext>(x => x.GetRequiredService<DbContext>());            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
