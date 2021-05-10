using BirthdayDashing.Domain.SeedWork;
using Common.Validation;
using System;

namespace BirthdayDashing.Domain.Dashing
{
    public class Dashing : Entity
    {        
        /// <summary>
        /// just for Mapping
        /// </summary>
        private Dashing()
        {            
        }
        public Dashing(Guid userId, DateTime birthday, string title, string postalCode, Guid createdById, string backgroundUUID, string name, DateTime currentYearBirthday, string city, string state)
        {
            Validate(nameof(Birthday), birthday, Validator.BirthdayIsValid);
            Validate(nameof(Title), postalCode, Validator.StringIsNotNullOrWhiteSpace);
            Validate(nameof(PostalCode), postalCode, Validator.StringIsNotNullOrWhiteSpace);
            Validate(nameof(PostalCode), postalCode, Validator.PostalCodeIsValid);

            UserId = userId;
            Birthday = birthday;
            Title = title;
            PostalCode = postalCode;
            DashingAmount = 0;
            Active = true;
            Deleted = false;
            LastEditById = null;
            LastEditDate = null;
            CreatedById = createdById;
            CreatedDate = DateTime.Now;
            BackgroundUUID = backgroundUUID;
            Name = name;
            CurrentYearBirthday = currentYearBirthday;
            City = city;
            State = state;
            TitleUpdated = false;
        }

        public void Update(string title, string postalCode, Guid lastEditById, string backgroundUUID, string name, DateTime currentYearBirthday, string city, string state)
        {            
            Validate(nameof(Title), postalCode, Validator.StringIsNotNullOrWhiteSpace);
            Validate(nameof(PostalCode), postalCode, Validator.StringIsNotNullOrWhiteSpace);
            Validate(nameof(PostalCode), postalCode, Validator.PostalCodeIsValid);

            TitleUpdated = !string.Equals(Title, title, StringComparison.OrdinalIgnoreCase);
            Title = title;
            PostalCode = postalCode;
            LastEditById = lastEditById;
            LastEditDate = DateTime.Now;
            BackgroundUUID = backgroundUUID;
            Name = name;
            CurrentYearBirthday = currentYearBirthday;
            City = city;
            State = state;
        }
        public bool ToggleActive(Guid lastEditById)
        {
            Active = !Active;
            LastEditById = lastEditById;
            LastEditDate = DateTime.Now;
            return Active;
        }

        public bool ToggleDeleted(Guid lastEditById)
        {
            Deleted = !Deleted;
            LastEditById = lastEditById;
            LastEditDate = DateTime.Now;
            return Deleted;
        }

        public Guid UserId { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Title { get; private set; }
        public string PostalCode { get; private set; }
        public decimal DashingAmount { get; private set; }
        public bool Active { get; private set; }
        public bool Deleted { get; private set; }
        public Guid? LastEditById { get; private set; }
        public DateTime? LastEditDate { get; private set; }
        public Guid CreatedById { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string BackgroundUUID { get; private set; }
        public string Name { get; private set; }
        public DateTime CurrentYearBirthday { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public bool TitleUpdated { get; private set; }
        public byte[] RowVersion { get; private set; }
    }
}
