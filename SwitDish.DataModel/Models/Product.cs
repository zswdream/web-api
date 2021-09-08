using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class Product
    {
        public Product()
        {
            CustomerOrderProducts = new HashSet<CustomerOrderProduct>();
        }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int VendorId { get; set; }
        public int ProductCategoryId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<CustomerOrderProduct> CustomerOrderProducts { get; set; }
    }
}
