using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class UpdateUserDto
    {
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }        
        [MaxLength(2048)]
        [RegularExpression(@"^\/+.*\.(jpg|jpeg|png|bmp|tif|gif)$")]
        public string ImageUrl { get; set; }
    }
}
