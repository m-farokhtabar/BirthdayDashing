using System;

namespace BirthdayDashing.Application.Configuration.Authorization
{
    public interface IAuthorizedUserDetails
    {
        public Guid UserId { get; }
        string[] Roles { get; }

    }
}
