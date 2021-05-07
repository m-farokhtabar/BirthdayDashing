using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.VerficationCodes.Input
{
    public class ForgotPasswordDto
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
