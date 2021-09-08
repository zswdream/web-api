using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class VendorFeature
    {
        public int VendorFeatureId { get; set; }
        public int VendorId { get; set; }
        public int FeatureId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Feature Feature { get; set; }
    }
}
