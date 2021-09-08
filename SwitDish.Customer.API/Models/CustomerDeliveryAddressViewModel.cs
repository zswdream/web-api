using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SwitDish.Customer.API.Models
{
    public class CustomerDeliveryAddressViewModel
    {
        public int CustomerDeliveryAddressId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        [MinLength(1)]
        public string Instructions { get; set; }
        [Required]
        [MinLength(1)]
        public string CompleteAddress { get; set; }
        [Required]
        [MinLength(1)]
        public string DeliveryArea { get; set; }
        [Required]
        [RegularExpression(@"\bHome\b|\bWork\b|\bOther\b", ErrorMessage = "Delivery Address Type can only be Home, Work or Other")]
        public string Type { get; set; }
    }
}
