using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class VendorServiceCategory
    {
        public int VendorServiceCategoryId { get; set; }
        public int ServiceCategoryId { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
    }
}
