using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class ResetPasswordDto
    {
        [Required]
        [MaxLength(20)]
        public string Token { get; set; }
        [Required]
        [MaxLength(100)]
        public string NewPassword { get; set; }
    }
}
