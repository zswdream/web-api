using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class VendorDeliveryTime
    {
        public int VendorDeliveryTimeId { get; set; }
        public TimeSpan Minimum { get; set; }
        public TimeSpan Maximum { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
