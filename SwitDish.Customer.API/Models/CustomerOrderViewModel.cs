using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SwitDish.Customer.API.Models
{
    public class CustomerOrderViewModel
    {
        [JsonPropertyName("customer_order_id")]
        public int CustomerOrderId { get; set; }

        [JsonPropertyName("customer_id")]   
        public int CustomerId { get; set; }

        [JsonPropertyName("customer_deliveryaddress_id")]
        public int CustomerDeliveryAddressId { get; set; }

        [JsonPropertyName("order_number")]
        public string OrderNumber { get; set; }

        [JsonPropertyName("ordered_on")]
        public DateTime OrderedOn { get; set; }
        
        [JsonPropertyName("edited_on")]
        public DateTime? EditedOn { get; set; }

        [JsonPropertyName("delivered_on")]
        public DateTime? DeliveredOn { get; set; }

        [JsonPropertyName("is_reviewed")]
        public bool IsReviewed { get; set; }

        [JsonPropertyName("restaurant_charges")]
        public decimal RestaurantCharges { get; set; }
        
        [JsonPropertyName("delivery_charges")]
        public decimal DeliveryCharges { get; set; }

        [JsonPropertyName("order_description")]
        public string OrderDescription { get; set; }

        [JsonPropertyName("order_status")]
        public string OrderStatus { get; set; }

        [JsonPropertyName("vendor_id")]
        public string VendorId { get; set; }

        [JsonPropertyName("vendor_name")]
        public string VendorName { get; set; }

        [JsonPropertyName("vendor_address")]
        public Address VendorAddress { get; set; }

        [JsonPropertyName("vendor_image")]
        public string VendorProfileImage { get; set; }

        [JsonPropertyName("sub_total")]
        public decimal SubTotal { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("grand_total")]
        public decimal GrandTotal { get; set; }

        [JsonPropertyName("customer_address")]
        public CustomerDeliveryAddressViewModel CustomerDeliveryAddress { get; set; }

        [JsonPropertyName("order_products")]
        public List<CustomerOrderProductViewModel> CustomerOrderProducts { get; set; }
    }
}
