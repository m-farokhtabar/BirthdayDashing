﻿using BirthdayDashing.API.Helper;
using BirthdayDashing.API.StartupConfig;
using BirthdayDashing.API.ViewModel;
using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Dtos.Users.Output;
using BirthdayDashing.Application.Dtos.VerficationCodes.Input;
using BirthdayDashing.Application.Users;
using BirthdayDashing.Application.VerificationCodes;
using Common.Exception;
using Common.Feedback;
using Common.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Common.Exception.Messages;

namespace BirthdayDashing.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserWriteService WriteService;
        private readonly IUserReadService ReadService;
        private readonly IWebHostEnvironment Host;
        private readonly IVerificationCodeWriteService VerificationCodeWriteService;
        private readonly AppSettings AppSettings;
        public AccountController(IUserWriteService writeService, IUserReadService readService, IWebHostEnvironment host, IOptions<AppSettings> appSettings, IVerificationCodeWriteService verificationCodeWriteService)
        {
            WriteService = writeService;
            ReadService = readService;
            Host = host;
            VerificationCodeWriteService = verificationCodeWriteService;
            AppSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost]
        public async Task<ActionResult<Feedback<bool>>> Register([FromBody] AddUserDto user)
        {
            await WriteService.AddAsync(user);
            return Ok(true);
        }
        
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost("ReSendConfirmEmail")]
        public async Task<ActionResult<Feedback<bool>>> ReSendConfirmEmail([FromBody] ReSendConfirmEmailDto confirmEmail)
        {
            await VerificationCodeWriteService.NewCodeForConfirmEmailAsync(confirmEmail);
            return Ok(true);
        }
        
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost("ConfirmByEmail")]
        public async Task<ActionResult<Feedback<bool>>> ConfirmByEmail(ConfirmUserByEmailDto confirmUser)
        {
            await WriteService.ConfirmByEmailAsync(confirmUser);
            return Ok(true);
        }
        
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost("Login")]
        public async Task<ActionResult<Feedback<AuthenticatedUserViewModel>>> Login(LoginDto login)
        {
            UserWithRolesNameDto userRolesInfo = await ReadService.GetAuthenticationDataByEmailAsync(login.Email);
            if (userRolesInfo is null)
                throw new ManualException(DATA_IS_INCORRECT.Replace("{0}", nameof(login.Email)), ExceptionType.NotFound, nameof(login.Email));
            if (!Common.Security.VerifyPassword(login.Password, userRolesInfo.Password))
                throw new ManualException(DATA_IS_INCORRECT.Replace("{0}", nameof(login.Password)), ExceptionType.NotFound, nameof(login.Password));

            if (userRolesInfo.Roles is null || userRolesInfo.Roles.Count == 0)
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "User");

            if (!userRolesInfo.IsApproved)
                throw new ManualException(USER_IS_NOT_APPROVED, ExceptionType.UnAuthorized, nameof(userRolesInfo.IsApproved));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.SigningKey);

            List<Claim> claims = new();
            claims.Add(new(ClaimTypes.Name, userRolesInfo.Id.ToString()));
            foreach (var Role in userRolesInfo.Roles)
                claims.Add(new(ClaimTypes.Role, Role.Name));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(AppSettings.UserExpiteTimeinDay),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new AuthenticatedUserViewModel()
            {
                Id = userRolesInfo.Id,
                Birthday = userRolesInfo.Birthday,                
                FirstName = userRolesInfo.FirstName,
                LastName = userRolesInfo.LastName,
                ImageUrl = userRolesInfo.ImageUrl,
                PhoneNumber = userRolesInfo.PhoneNumber,
                PostalCode = userRolesInfo.PostalCode,
                Token = tokenHandler.WriteToken(token)
            });
        }

        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<Feedback<bool>>> ForgotPassword([FromBody] ForgotPasswordDto forgetPassword)
        {
            await VerificationCodeWriteService.NewCodeForForgotPasswordAsync(forgetPassword);
            return Ok(true);
        }
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpGet("{id}")]        
        public async Task<ActionResult<Feedback<UserDto>>> Get(Guid id)
        {
            //TODO: Only allow admins to access other user records
            //var currentUserId = int.Parse(User.Identity.Name);
            //if (id != currentUserId && !User.IsInRole(Role.Admin))
            //    return Forbid();

            return Ok(await ReadService.GetAsync(id));
        }
        
        [HttpPost("UserProfilePicture")]        
        public async Task<ActionResult<Feedback<string>>> UploadUserProfilePicture([Required][ImageValidator(5242880, "jpg|jpeg|png|bmp|tif|gif")] IFormFile picture)
        {
            return Ok(await (new ManageFiles(Host)).Save(picture, "User", "UserProfileImage"));
        }        
        
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut("{id}")]
        public async Task<ActionResult<Feedback<bool>>> Update(Guid id, [FromBody] UpdateUserDto user)
        {
            await WriteService.UpdateAsync(id, user);
            return Ok(true);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut("ChangePassword/{id}")]
        public async Task<ActionResult<Feedback<bool>>> ChangePassword(Guid id, [FromBody] ChangePasswordDto password)
        {
            await WriteService.ChangePasswordAsync(id, password);
            return Ok(true);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut("ResetPassword/{id}")]
        public async Task<ActionResult<Feedback<bool>>> ResetPassword(Guid id, [FromBody] ResetPasswordDto password)
        {
            await WriteService.ResetPasswordAsync(id, password);
            return Ok(true);
        }
    }
}
