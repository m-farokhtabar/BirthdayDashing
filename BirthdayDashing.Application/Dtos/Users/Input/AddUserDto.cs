using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class AddUserDto
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
    }
}
