using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Common.ViewModels
{
    public class PasswordUpdateViewModel
    {
        [JsonPropertyName("user_id")]
        [Required]
        public int ApplicationUserId{ get; set; }

        [JsonPropertyName("old_password")]
        [Required]
        public string OldPassword { get; set; }

        [JsonPropertyName("new_password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password too short. Minimum length is 8")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "Password must contain both digits and characters")]
        public string NewPassword { get; set; }

        [JsonPropertyName("confirm_password")]
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "New Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }
}
