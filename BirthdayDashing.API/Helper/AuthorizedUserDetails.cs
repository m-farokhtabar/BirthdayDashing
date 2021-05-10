using BirthdayDashing.Application.Configuration.Authorization;
using Common.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using static Common.Exception.Messages;

namespace BirthdayDashing.API.Helper
{
    public class AuthorizedUserDetails : IAuthorizedUserDetails
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        public AuthorizedUserDetails(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {                
                try
                {
                    return Guid.Parse(HttpContextAccessor.HttpContext.User.Identity.Name);
                }
                catch
                {                    
                    throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "User");
                }
            }

        }
    public string[] Roles => HttpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Type == ClaimTypes.Role)?.Select(x => x.Value)?.ToArray();

}
}
