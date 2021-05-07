using BirthdayDashing.Domain.Base;
using Common;
using Common.Exception;
using Common.Validation;
using System;
using System.Collections.Generic;
using static Common.Exception.Messages;

namespace BirthdayDashing.Domain
{
    public class User : Entity
    {
        /// <summary>
        /// just for Mapping
        /// </summary>
        private User()
        {

        }
        public User(string email, string password, string postalCode, DateTime birthday, Guid RoleId, string firstName = null, string lastName = null, string phoneNumber = null, string imageUrl = null)
        {
            Validate(nameof(Email), email, Validator.EmailIsValid);
            Validate(nameof(Password), password, Validator.StringIsNotNullOrWhiteSpace);
            Validate(nameof(PostalCode), postalCode, Validator.StringIsNotNullOrWhiteSpace);
            Validate(nameof(Birthday), birthday, Validator.BirthdayIsValid);

            Email = email;
            Password = Security.HashPassword(password);
            PostalCode = postalCode;
            Birthday = birthday;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            ImageUrl = imageUrl;
            userRoles = new List<UserRole>() { new UserRole(Id, RoleId) { } };
            IsApproved = false;
        }
        public void Update(string postalCode, DateTime birthday, string firstName, string lastName, string phoneNumber, string imageUrl)
        {
            Validate(nameof(Birthday), birthday, Validator.BirthdayIsValid);
            Validate(nameof(PostalCode), postalCode, Validator.StringIsNotNullOrWhiteSpace);

            PostalCode = postalCode;
            Birthday = birthday;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            ImageUrl = imageUrl;
        }
        public void UpdatePassword(string newPassword, string oldPassword)
        {
            if (Security.VerifyPassword(oldPassword, Password))
            {
                Validate(nameof(Password), newPassword, Validator.StringIsNotNullOrWhiteSpace);
                Password = Security.HashPassword(newPassword);
            }
            else
                throw new ManualException(DATA_IS_WRONG.Replace("{0}", nameof(Password)), ExceptionType.InValid, nameof(Password));
        }
        public void Approved()
        {
            IsApproved = true;
        }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime Birthday { get; private set; }
        public string PostalCode { get; private set; }
        public string PhoneNumber { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string ImageUrl { get; private set; }
        public bool IsApproved { get; private set; }
        private List<UserRole> userRoles;
        public IReadOnlyCollection<UserRole> UserRoles => userRoles?.AsReadOnly();
        [System.ComponentModel.DataAnnotations.Timestamp]
        public byte[] RowVersion { get; private set; }
    }
}
