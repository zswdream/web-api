using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class VendorServiceCategory
    {
        public int VendorServiceCategoryId { get; set; }
        public int VendorId { get; set; }
        public int ServiceCategoryId { get; set; }
        public ServiceCategory ServiceCategory { get; set; }
    }
}
