using BirthdayDashing.API.Helper;
using BirthdayDashing.API.StartupConfig;
using BirthdayDashing.Application.Configuration.StartupConfig;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.StartupConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {                    
                    return new BadRequestObjectResult(CustomBadRequest.ConstructErrorMessages(context));
                };
            });
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
        /// <param name="srp"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider srp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwashBuckleSwagger();
            }            
            app.UseCustomizeExceptionHandler(srp.GetService<IManageDbExceptionUniqueAndKeyFields>());
            //app.UseStatusCodePages(async context =>
            //{
            //    if (context.HttpContext.Request.Path.StartsWithSegments("/api") &&
            //       (context.HttpContext.Response.StatusCode == 401 ||
            //        context.HttpContext.Response.StatusCode == 403))
            //    {
            //        //await context.HttpContext.Response.WriteAsync("Unauthorized request");
            //    }
            //});

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
