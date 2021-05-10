using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class ResetPasswordDto
    {
        [Required]
        [StringLength(20)]
        public string Token { get; set; }
        [Required]
        [StringLength(100)]
        public string NewPassword { get; set; }
    }
}
