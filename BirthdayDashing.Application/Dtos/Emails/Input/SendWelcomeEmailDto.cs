using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Emails.Input
{
    public class SendWelcomeEmailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
