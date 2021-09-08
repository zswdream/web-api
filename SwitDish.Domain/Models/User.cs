using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public int? AccountId { get; set; }
        public int? AddressId { get; set; }
        public int RoleId { get; set; }
    }
}
