using BirthdayDashing.Application.Dtos.Roles.Output;
using System;
using System.Collections.Generic;

namespace BirthdayDashing.Application.Dtos.Users.Output
{
    public class UserEssentialDataDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public List<RoleNameDto> Roles { get; set; }
    }
}
