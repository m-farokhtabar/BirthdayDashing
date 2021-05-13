using System;

namespace BirthdayDashing.Application.Authorization
{
    public interface IAuthorizationService
    {
        Guid UserId { get; }
        void Authorized(Guid OwnerId);
        void JustOwnerAuthorized(Guid? OwnerId);
    }
}