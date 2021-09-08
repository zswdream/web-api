using SwitDish.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class CustomerOrder
    {
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
        public Customer Customer { get; set; }
        public Vendor Vendor { get; set; }
        public VendorOffer VendorOffer { get; set; }
        public CustomerDeliveryAddress CustomerDeliveryAddress { get; set; }
        public CustomerOrderFeedback CustomerOrderFeedback { get; set; }
        public List<CustomerOrderProduct> CustomerOrderProducts { get; set; }
        public decimal SubTotal => CustomerOrderProducts.Sum(d => d.Quantity * d.Product.Price);
        public decimal Discount { get
            {
                var discount = 0m;

                if (VendorOffer != null)
                    discount = SubTotal * (VendorOffer.DiscountPercentage / 100);
                
                if (SubTotal >= 10000)
                    discount += SubTotal * 0.02m;

                return discount;
            } 
        }
        public decimal GrandTotal => SubTotal - Discount + Tax + DeliveryCharges;
        public string OrderDescription => String.Join(',' , CustomerOrderProducts.Select(d => $"{d.Product.Name} x{d.Quantity}"));
    }
}
