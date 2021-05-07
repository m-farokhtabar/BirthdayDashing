using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.VerficationCodes.Input
{
    public class ReSendConfirmEmailDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
