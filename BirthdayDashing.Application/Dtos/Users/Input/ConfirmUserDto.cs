using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class ConfirmUserDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Token { get; set; }        
    }
}
