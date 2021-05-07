using System;

namespace BirthdayDashing.API.ViewModel
{
    public class AuthenticatedUserViewModel
    {
        public Guid Id { get; set; }        
        public DateTime Birthday { get; set; }
        public string PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public string Token { get; set; }
    }
}
