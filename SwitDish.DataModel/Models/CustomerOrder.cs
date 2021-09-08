using SwitDish.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class CustomerOrder
    {
        public CustomerOrder()
        {
            CustomerOrderProducts = new HashSet<CustomerOrderProduct>();
        }
        public int CustomerOrderId { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public int CustomerDeliveryAddressId { get; set; }
        public int? VendorOfferId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderedOn { get; set; }
        public DateTime? EditedOn { get; set; }
        public DateTime? DeliveredOn { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Tax { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal RestaurantCharges { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual VendorOffer VendorOffer { get; set; }
        public virtual CustomerDeliveryAddress CustomerDeliveryAddress { get; set; }
        public virtual CustomerOrderFeedback CustomerOrderFeedback { get; set; }
        public virtual ICollection<CustomerOrderProduct> CustomerOrderProducts { get; set; }
    }
}
