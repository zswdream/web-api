using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class Product
    {
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public ProductCategory ProductCategory { get; set; }
        //[JsonIgnore]
        //public List<CustomerOrderProduct> CustomerOrderProducts { get; set; }
    }
}
