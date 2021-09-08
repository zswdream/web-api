using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class VendorFeature
    {
        public int VendorFeatureId { get; set; }
        public int VendorId { get; set; }
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
