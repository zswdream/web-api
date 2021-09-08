using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class VendorCuisine
    {
        public int VendorCuisineId { get; set; }
        public int CuisineId { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Cuisine Cuisine { get; set; }
    }
}
