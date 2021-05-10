using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class LoginDto
    {
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}