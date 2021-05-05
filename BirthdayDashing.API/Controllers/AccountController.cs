using BirthdayDashing.API.Helper;
using BirthdayDashing.API.StartupConfig;
using BirthdayDashing.API.ViewModel;
using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Dtos.Users.Output;
using BirthdayDashing.Application.Requests.Read.Users;
using BirthdayDashing.Application.Requests.Write.Users;
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
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayDashing.API.Controllers
{    
    public class AccountController : BaseController
    {
        private readonly IUserWriteService WriteService;
        private readonly IUserReadService ReadService;
        private readonly IWebHostEnvironment Host;
        private readonly AppSettings AppSettings;
        public AccountController(IUserWriteService writeService, IUserReadService readService, IWebHostEnvironment host, IOptions<AppSettings> appSettings)
        {
            WriteService = writeService;
            ReadService = readService;
            Host = host;
            AppSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]        
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<ActionResult<Feedback<bool>>> Register([FromBody] AddUserDto user)
        {
            await WriteService.AddAsync(user);
            return Ok(true);
        }
        
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status404NotFound)]
        [HttpPost("ConfirmByEmail")]
        public async Task<ActionResult<Feedback<bool>>> ConfirmByEmail(ConfirmUserDto confirmUser)
        {
            await WriteService.ConfirmByEmailAsync(confirmUser);
            return Ok(true);
        }

        
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status404NotFound)]
        [HttpPost("Login")]        
        public async Task<ActionResult<Feedback<AuthenticatedUserViewMoel>>> Login(LoginDto login)
        {
            UserWithRolesNameDto userRolesInfo = await ReadService.GetAuthentocateDataAsync(login);
            if (userRolesInfo is null || !Common.Security.VerifyPassword(login.Password, userRolesInfo.Password))
                throw new Exception("User or password is incorrect");
            if (userRolesInfo.Roles is null || userRolesInfo.Roles.Count == 0)
                throw new Exception("User is not authorized");

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

            return Ok(new AuthenticatedUserViewMoel() { Id = userRolesInfo.Id, Token = tokenHandler.WriteToken(token) });
        }
        
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status403Forbidden)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback<UserDto>>> Get(Guid id)
        {
            //TODO: Only allow admins to access other user records
            //var currentUserId = int.Parse(User.Identity.Name);
            //if (id != currentUserId && !User.IsInRole(Role.Admin))
            //    return Forbid();

            return Ok(await ReadService.Get(id));
        }
        
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status413RequestEntityTooLarge)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status403Forbidden)]
        [HttpPost("UserProfilePicture")]
        public async Task<ActionResult<Feedback<string>>> UploadUserProfilePicture([ImageValidator(5242880, "jpg|jpeg|png|bmp|tif|gif")] IFormFile picture)
        {
            return Ok(await (new ManageFiles(Host)).Save(picture, "User", "UserProfileImage"));
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<ActionResult<Feedback<bool>>> Update(Guid id, [FromBody] UpdateUserDto user)
        {
            await WriteService.UpdateAsync(id, user);
            return Ok(true);
        }
    }
}
