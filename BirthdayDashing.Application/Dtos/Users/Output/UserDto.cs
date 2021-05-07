using System;

namespace BirthdayDashing.Application.Dtos.Users.Output
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
    }
}
