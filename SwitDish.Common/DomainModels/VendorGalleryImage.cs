using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class VendorGalleryImage
    {
        public int VendorGalleryImageId { get; set; }
        public int VendorId { get; set; }
        public string Image{ get; set; }
    }
}
