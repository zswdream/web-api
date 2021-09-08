using System;

namespace SwitDish.Vendor.API.Models
{
    public class AgentVM
    {
        public int AgentId { get; set; }
        public int VendorAgentId { get; set; }
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
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime? CreatedDates { get; set; }
        public string Role { get; set; }
        public bool Selected { get; set; }
    }
}
