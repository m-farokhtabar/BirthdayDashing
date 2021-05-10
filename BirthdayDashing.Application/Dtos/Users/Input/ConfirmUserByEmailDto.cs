using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class ConfirmUserByEmailDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string Token { get; set; }
    }
}
