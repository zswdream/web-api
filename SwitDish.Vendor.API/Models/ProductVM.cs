using System;

namespace SwitDish.Vendor.API.Models
{
    public class ProductVM
    {

        public long ProductId { get; set; }
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreateDate { get; set; }
        //public string CreatedDate { get; set; }

    }
}
