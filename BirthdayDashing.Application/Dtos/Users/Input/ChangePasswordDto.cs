using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class ChangePasswordDto
    {
        [Required]
        [StringLength(100)]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(100)]
        public string NewPassword { get; set; }
    }
}
