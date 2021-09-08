using System;

namespace SwitDish.Vendor.API.Models
{
    public class VendorVM
    {
        public long VendorId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string VendorDate { get; set; }
        public long AddressId { get; set; }
        public string Address { get; set; }
        public bool? IsVendorDeliver { get; set; }
    }
}
