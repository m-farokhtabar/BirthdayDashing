using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Users.Input
{
    public class UpdateUserDto
    {
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(2048)]
        //TODO: ImageUrl without url validaition maybe makes some security problems
        //[RegularExpression(@"^\/+.*\.(jpg|jpeg|png|bmp|tif|gif)$")]
        public string ImageUrl { get; set; }
    }
}
