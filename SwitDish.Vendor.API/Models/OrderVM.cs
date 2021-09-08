using System;

namespace SwitDish.Vendor.API.Models
{
    public class OrderVM
    {
        public long OrderId { get; set; }
        //public System.DateTime Date { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> PaymentType { get; set; }
        public System.String PaymentTypeName { get; set; }
        public string Description { get; set; }
        public long VendorEmployeeId { get; set; }
        public string VendorEmployeeName { get; set; }
        public Nullable<int> RewardId { get; set; }
        public string RewardName { get; set; }
        public Nullable<long> ClaimId { get; set; }
        public string ClaimName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> OrderStatus { get; set; }
    }
}
