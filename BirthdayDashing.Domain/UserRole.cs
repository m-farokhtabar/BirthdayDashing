using System;

namespace BirthdayDashing.Domain
{
    public class UserRole
    {
        /// <summary>
        /// just for Mapping
        /// </summary>
        private UserRole()
        {

        }

        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }
    }
}
