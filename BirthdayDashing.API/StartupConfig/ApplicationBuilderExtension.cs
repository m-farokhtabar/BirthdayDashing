using BirthdayDashing.Infrastructure.Data.Write;
using Common.Exception;
using Common.Feedback;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mime;
using static Common.Exception.Messages;

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
        public static void UseCustomizeExceptionHandler(this IApplicationBuilder app, IManageDbExceptionUniqueAndKeyFields ManageDbExceptionUniqueAndKeyFields)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    int StatusCode = (int)HttpStatusCode.InternalServerError;
                    string ContentType = "application/json; charset=utf-8";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature is not null)
                    {
                        Feedback<bool> Fb = null;
                        switch (contextFeature.Error)
                        {
                            case ManualException CurrentException:
                                StatusCode = (int)CurrentException.Type;
                                Fb = new Feedback<bool>(false, MessageType.LogicalError, CurrentException.Message, CurrentException.Parameter);
                                break;
                            case SqlException CurrentException:
                                switch (CurrentException.Number)
                                {
                                    //Unique field is already exist
                                    case 2601:
                                    //Primary key is already exist
                                    case 2627:
                                        string UniqueParameterName = ManageDbExceptionUniqueAndKeyFields.FindUniqueOrKeyFieldsInMessage(CurrentException.Message);
                                        StatusCode = (int)HttpStatusCode.Conflict;
                                        Fb = new Feedback<bool>(false, MessageType.LogicalError, DATA_IS_ALREADY_EXIST.Replace("{0}", UniqueParameterName), UniqueParameterName);
                                        break;
                                    //There is not the primary key to use as a foreign key 
                                    case 547:
                                        string KeyParameterName = ManageDbExceptionUniqueAndKeyFields.FindUniqueOrKeyFieldsInMessage(CurrentException.Message);
                                        StatusCode = (int)HttpStatusCode.NotFound;
                                        Fb = new Feedback<bool>(false, MessageType.LogicalError, DATA_IS_NOT_FOUND.Replace("{0}", KeyParameterName), KeyParameterName);
                                        break;
                                }
                                break;
                        }
                        context.Response.Clear();
                        context.Response.ContentType = ContentType;
                        context.Response.StatusCode = StatusCode;
                        Fb ??= new Feedback<bool>(false, MessageType.RuntimeError, contextFeature?.Error?.StackTrace == null ? INTERNAL_ERROR : contextFeature?.Error?.StackTrace, contextFeature?.Error?.Message);
                        await context.Response.WriteAsync(Fb.ToString());
                    }
                });
            });
        }

        public static void UseCustomizeUseStatusCodePages(this IApplicationBuilder app)
        {
            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
                string Message;
                string Key;
                switch (context.HttpContext.Response.StatusCode)
                {
                    case 401:
                        Message = USER_IS_NOT_AUTHORIZED;
                        Key = "User";
                        break;
                    case 403:
                        Message = USER_IS_NOT_AUTHORIZED;
                        Key = "User";
                        break;
                    default:
                        Message = "Runtime error has been occured!";
                        Key = context.HttpContext.Response.StatusCode.ToString();
                        break;
                }

                Feedback<bool> Fb = new(false, MessageType.RuntimeError, Message, Key);
                await context.HttpContext.Response.WriteAsync(Fb.ToString());                
            });
        }
    }
}
