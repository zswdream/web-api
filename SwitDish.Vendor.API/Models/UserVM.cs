using System;

namespace SwitDish.Vendor.API.Models
{
    public class UserVM
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Active { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public Nullable<long> AccountId { get; set; }
        public string authToken { get; set; }
        public string idToken { get; set; }
        public string accessToken { get; set; }
        public string authMethod { get; set; }
        public Nullable<int> rating { get; set; }
        public string UserDate { get; set; }
    }
}
