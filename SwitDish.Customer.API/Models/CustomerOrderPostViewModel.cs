using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class CustomerOrderPostViewModel
    {
        [Required]
        public int CustomerId { get; set; }
        public int CustomerOrderId { get; set; }
        [Required]
        public int CustomerDeliveryAddressId { get; set; }
        [Required]
        public int VendorId { get; set; }
        public int VendorOfferId { get; set; }
        public List<CustomerOrderProductViewModel> CustomerOrderProducts { get; set; }
    }
}
