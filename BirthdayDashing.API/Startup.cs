using BirthdayDashing.API.StartupConfig;
using BirthdayDashing.Application.StartupConfig;
using BirthdayDashing.Infrastructure.StartupConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace BirthdayDashing.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.ConfigureSwashBuckleSwagger();
            services.ConfigureAuthentication(Configuration);
            services.ConfigureDataAccess(Configuration["ConnectionStrings:" + "Connection"]);
            services.ConfigureRepositories();
            services.ConfigureServices();
            services.ConfigureHostAddresses();
            services.ConfigureEmailSetting(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwashBuckleSwagger();
            }

            app.UseHttpsRedirection();
            app.UseStaticFilesAllowUploadedFile(env);
            app.UseRouting();
            app.UseCorsAllowAny();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
