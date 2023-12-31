﻿using BirthdayDashing.API.Helper;
using BirthdayDashing.Application.Configuration.Authorization;
using BirthdayDashing.Application.Configuration.Email;
using BirthdayDashing.Application.Configuration.Setting;
using BirthdayDashing.Infrastructure.StartupConfig;
using Common.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace BirthdayDashing.API.StartupConfig
{
    /// <summary>
    /// Startup Configure Services
    /// </summary>
    public static class ServiceExtension
    {
        public static void ConfigureSwashBuckleSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0.0", new OpenApiInfo
                {
                    Title = "BirthdayDashing Services",
                    Version = "v1.0.0",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "",
                        Email = "",
                        Url = null
                    }
                });
                var XmlAPI = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(XmlAPI);
            });
        }
        public static AppSettings ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSetting = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSetting);
            AppSettings appSettings = appSettingsSetting.Get<AppSettings>();
            
            Validator.MaxAge = appSettings.MaxAge;
            Validator.MinAge = appSettings.MinAge;
            
            services.AddSingleton<ISettings, AppSettings>(x=> appSettings);

            return appSettings;
        }
        public static void ConfigureAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SigningKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
        public static void ConfigureCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowCredentials()
                    .SetIsOriginAllowed(x => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
        public static void ConfigureHostAddressesAndAuthorizedUser(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHostAddresses, HostAddresses>();
            services.AddSingleton<IAuthorizedUserDetails, AuthorizedUserDetails>();
        }
        public static void ConfigureEmailSetting(this IServiceCollection services, IConfiguration configuration)
        {
            var EmailSetting = configuration.GetSection("EmailSetting").Get<EmailSetting>();
            services.AddSingleton<IEmailSetting, EmailSetting>(x=> EmailSetting);
            services.ConfigureEmail();
        }        
    }
}
