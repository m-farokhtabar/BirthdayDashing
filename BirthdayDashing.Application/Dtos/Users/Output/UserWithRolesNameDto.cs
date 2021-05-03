using System;
using System.Collections.Generic;

namespace BirthdayDashing.Application.Dtos.Users.Output
{
    public class UserWithRolesNameDto
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public List<RoleNameDto> Roles { get; set; }
    }
    public class RoleNameDto
    {
        public string Name { get; set; }
    }
}
