using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class CustomerBookingViewModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public DateTime DateAndTime { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int VendorId { get; set; }
    }
}
