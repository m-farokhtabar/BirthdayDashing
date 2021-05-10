using BirthdayDashing.Application.Configuration.Authorization;
using BirthdayDashing.Domain.Roles;
using Common.Exception;
using System;
using System.Linq;
using static Common.Exception.Messages;

namespace BirthdayDashing.Application.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizedUserDetails AuthorizedUserDetails;
        public Guid UserId => AuthorizedUserDetails.UserId;
        public AuthorizationService(IAuthorizedUserDetails authorizedUserDetails)
        {
            AuthorizedUserDetails = authorizedUserDetails;
        }

        public void Authorized(Guid OwnerId)
        {
            if (AuthorizedUserDetails.Roles is null || AuthorizedUserDetails.Roles.Length == 0)
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "User");

            if (AuthorizedUserDetails.UserId != OwnerId && !AuthorizedUserDetails.Roles.Any(x => x == Role.Admin))
                throw new ManualException(USER_IS_NOT_AUTHORIZED, ExceptionType.UnAuthorized, "User");
        }
    }
}
