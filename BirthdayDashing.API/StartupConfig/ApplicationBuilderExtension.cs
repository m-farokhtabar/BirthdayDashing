using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace BirthdayDashing.API.StartupConfig
{
    /// <summary>
    /// configure the HTTP request pipeline
    /// </summary>
    public static class ApplicationBuilderExtension
    {
        public static void UseSwashBuckleSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {                
                c.SwaggerEndpoint("/swagger/v1.0.0/swagger.json", "BirthdayDashing Services v1.0.0");
            });
        }
        public static void UseCorsAllowAny(this IApplicationBuilder app)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
        public static void UseStaticFilesAllowUploadedFile(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "Upload")),
                RequestPath = "/Upload"
            });
        }
    }
}
