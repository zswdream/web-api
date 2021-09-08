using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class CustomerFavouriteVendor
    {
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
