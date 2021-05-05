using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class ChangePasswordDto
    {
        [Required]
        [MaxLength(100)]
        public string OldPassword { get; set; }
        [Required]
        [MaxLength(100)]
        public string NewPassword { get; set; }
    }
}
