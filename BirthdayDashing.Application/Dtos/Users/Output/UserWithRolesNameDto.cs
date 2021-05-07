using System.Collections.Generic;

namespace BirthdayDashing.Application.Dtos.Users.Output
{
    public class UserWithRolesNameDto : UserDto
    {        
        public string Password { get; set; }
        public bool IsApproved { get; set; }
        public List<RoleNameDto> Roles { get; set; }
    }
    public class RoleNameDto
    {
        public string Name { get; set; }
    }
}
