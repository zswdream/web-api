using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SwitDish.Customer.API.Models
{
    public class CustomerProfileUpdateViewModel
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        [MinLength(1)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(1)]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email address is invalid")]
        public string Email { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string Phone { get; set; }
    }
}
