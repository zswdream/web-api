using System;

namespace SwitDish.Vendor.API.Models
{
    public class PromotionProductVM
    {

        public int Id { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> promotionId { get; set; }
        public string PromotionName { get; set; }
        public Nullable<System.DateTime> DateExpired { get; set; }
        public string DateExpire { get; set; }
        public Nullable<int> userId { get; set; }
        public string username { get; set; }
    }
}
