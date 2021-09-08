using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class SignUpViewModel
    {
        [Required]
        [MinLength(1)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(1)]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email address is invalid")]
        public string EmailAddress { get; set; }
        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string MobileNo { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password too short. Minimum length is 8")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "Password must be at least 8 alphanumeric characters")]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }
}
