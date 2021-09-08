using System;

namespace SwitDish.Common.DomainModels
{
    public class ApplicationUser
    {
        public int ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
