using System;

namespace BirthdayDashing.Application.Dtos.VerficationCodes.Input
{
    public class NewConfirmEmailDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }        
    }
}
