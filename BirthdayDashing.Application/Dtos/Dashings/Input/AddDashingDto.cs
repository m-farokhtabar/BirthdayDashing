using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Dashings.Input
{
    public class AddDashingDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [StringLength(256)]
        public string Title { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }
        [StringLength(2048)]
        public string BackgroundUUID { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        public DateTime CurrentYearBirthday { get; set; }
        [StringLength(45)]
        public string City { get; set; }
        [StringLength(45)]
        public string State { get; set; }
    }
}
