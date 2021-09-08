using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class VendorCuisine
    {
        public int VendorCuisineId { get; set; }
        public int VendorId { get; set; }
        public int CuisineId { get; set; }
        public Cuisine Cuisine { get; set; }
    }
}
