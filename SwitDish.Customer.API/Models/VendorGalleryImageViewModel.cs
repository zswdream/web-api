using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Customer.API.Models
{
    public class VendorGalleryImageViewModel
    {
        public int VendorGalleryImageId { get; set; }
        public string Image { get; set; }
        public int VendorId { get; set; }
    }
}
