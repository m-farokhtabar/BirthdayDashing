using BirthdayDashing.API.Helper;
using BirthdayDashing.API.StartupConfig;
using BirthdayDashing.API.ViewModel;
using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Dtos.Users.Output;
using BirthdayDashing.Application.Requests.Read.Users;
using BirthdayDashing.Application.Requests.Write.Users;
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
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]    
    public class AccountController : Controller
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
        [HttpPost]
        public async Task<Boolean> Register([FromBody] AddUserDto user)
        {
            await WriteService.AddAsync(user);
            return true;
        }

        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost("ConfirmByEmail")]
        public async Task ConfirmByEmail(ConfirmUserDto confirmUser)
        {
            await WriteService.ConfirmByEmailAsync(confirmUser);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<AuthenticatedUserViewMoel> Login(LoginDto login)
        {
            var userRolesInfo = await ReadService.GetAuthentocateDataAsync(login);
            if (userRolesInfo is null)
                throw new Exception("User is not found");
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

            return new AuthenticatedUserViewMoel() { Id = userRolesInfo.Id, Token = tokenHandler.WriteToken(token) };            
        }  
        
        [HttpGet("{id}")]
        public async Task<UserDto> Get(Guid id)
        {
            //TODO: Only allow admins to access other user records
            //var currentUserId = int.Parse(User.Identity.Name);
            //if (id != currentUserId && !User.IsInRole(Role.Admin))
            //    return Forbid();

            return await ReadService.Get(id);        
        }        
        [HttpPost("UserProfilePicture")]
        public async Task<string> UploadUserProfilePicture([ImageValidator(5242880, "jpg|jpeg|png|bmp|tif|gif")] IFormFile picture)
        {
            return await (new ManageFiles(Host)).Save(picture, "User", "UserProfileImage");
        }                
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] UpdateUserDto user)
        {
            await WriteService.UpdateAsync(id, user);
        }
    }
}
