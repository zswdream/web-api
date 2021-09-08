using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SwitDish.Customer.API.Models
{
    public class UserSecurityQuestionViewModel
    {
        [Required]
        public int SecurityQuestionId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        [MinLength(1)]
        public string Answer { get; set; }
    }
}
