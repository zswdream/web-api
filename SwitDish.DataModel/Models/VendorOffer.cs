using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class VendorOffer
    {
        public int VendorOfferId { get; set; }
        public int VendorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string OfferCode { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public bool IsActive { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
