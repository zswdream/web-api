using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class CustomerBooking
    {
        public int CustomerBookingId { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DateTime { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
