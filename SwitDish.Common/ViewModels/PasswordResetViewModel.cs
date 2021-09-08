using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Common.ViewModels
{
    public class PasswordResetViewModel
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("verification_code")]
        public string VerificationCode { get; set; }
        [JsonPropertyName("new_password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "New Password too short. Minimum length is 8")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "New Password must be at least 8 alphanumeric characters")]
        public string NewPassword { get; set; }
        [JsonPropertyName("confirm_password")]
        [Compare(nameof(NewPassword), ErrorMessage = "New Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }
}
