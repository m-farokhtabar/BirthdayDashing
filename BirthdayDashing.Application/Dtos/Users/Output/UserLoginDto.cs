using BirthdayDashing.Application.Dtos.Roles.Output;
using System;
using System.Collections.Generic;

namespace BirthdayDashing.Application.Dtos.Users.Output
{
    public class UserLoginDto
    {
        public Guid Id { get; set; }
        public DateTime Birthday { get; set; }
        public string PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public List<RoleNameDto> RolesName { get; set; }
    }
}
