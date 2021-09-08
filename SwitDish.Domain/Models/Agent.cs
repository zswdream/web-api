using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Models
{
    public class Agent
    {
        public long AgentId { get; set; }
        public string AgentFirstName { get; set; }
        public string AgentLastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public int RoleId { get; set; }
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public string AgentCompanyDescription { get; set; }
        public string AgentCompanyName { get; set; }
        public string VendorName { get; set; }
        public string DelieveryCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
