using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class CustomerOrderProductViewModel
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string ProductDescription { get; set; }
    }
}
