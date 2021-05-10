using System;

namespace BirthdayDashing.Application.Dtos.Dashings.Output
{
    public class DashingDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Birthday { get; set; }
        public string Title { get; set; }
        public string PostalCode { get; set; }
        public decimal DashingAmount { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string BackgroundUUID { get; set; }
        public string Name { get; set; }
        public DateTime CurrentYearBirthday { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool TitleUpdated { get; set; }
    }
}
