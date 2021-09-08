using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class CustomerFavouriteVendor
    {
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public Customer Customer { get; set; }
        public Vendor Vendor { get; set; }
    }
}
