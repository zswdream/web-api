using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Customer.API.Models
{
    public class VendorOfferViewModel
    {
        public int VendorOfferId { get; set; }
        public int VendorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public bool IsActive { get; set; }
    }
}
