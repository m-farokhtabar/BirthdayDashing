using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Emails.Input
{
    public class SendForgotPasswordEmailDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string Token { get; set; }
    }
}
