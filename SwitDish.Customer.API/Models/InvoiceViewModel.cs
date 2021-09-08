using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class InvoiceViewModel
    {
        public long Id { get; set; }
        public string OrderNo { get; set; }
        public string OrderPlacedDateTime { get; set; }
        public string OrderDeliveredDateTime { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveredTo { get; set; }
        public string DeliveredAt { get; set; }
        public string OrderFrom { get; set; }
        public string OrderAddress { get; set; }
        public decimal ProductTotal { get; set; }
        public decimal GST { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal GrandTotal { get; set; }
        public ICollection<ProductViewModel> Products { get; set; }
    }
}
