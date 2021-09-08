using System;

namespace SwitDish.Vendor.API.Models
{
    public class PromotionVM
    {

        public System.Int32 PromotionId { get; set; }
        public System.String Name { get; set; }
        public System.String uniqueReference { get; set; }
        public string DateCreate { get; set; }
        public Nullable<System.DateTime> DateExpired { get; set; }
        public System.String CreatedBy { get; set; }
        public Nullable<bool> isUsed { get; set; }
    }
}
