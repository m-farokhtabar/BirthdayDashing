using BirthdayDashing.API.Helper;
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
using System.ComponentModel.DataAnnotations.Schema;
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
        private Guid UserId => Guid.Parse(User.Identity.Name);

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

        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut("ResetPassword/{id}")]
        public async Task<ActionResult<Feedback<bool>>> ResetPassword(Guid id, [FromBody] ResetPasswordDto password)
        {
            await WriteService.ResetPasswordAsync(id, password);
            return Ok(true);
        }
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpGet]
        public async Task<ActionResult<Feedback<UserDto>>> Get()
        {
            var Value = await ReadService.GetAsync(UserId);
            if (Value != null)
                return Ok(Value);
            else
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "User"), ExceptionType.NotFound, "UserId");
        }

        [HttpPost("UserProfilePicture")]
        public async Task<ActionResult<Feedback<string>>> UploadUserProfilePicture([Required][ImageValidator(5242880, "jpg|jpeg|png|bmp|tif|gif")] IFormFile picture)
        {
            //TODO: Add UserId to name of file
            return Ok(await (new ManageFiles(Host)).Save(picture, "User", "UserProfileImage"));
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut]
        public async Task<ActionResult<Feedback<bool>>> Update([FromBody] UpdateUserDto user)
        {
            await WriteService.UpdateAsync(UserId, user);
            return Ok(true);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut("ChangePassword")]
        public async Task<ActionResult<Feedback<bool>>> ChangePassword([FromBody] ChangePasswordDto password)
        {
            await WriteService.ChangePasswordAsync(UserId, password);
            return Ok(true);
        }        
    }
}
