using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class VendorInfoViewModel
    {
        [JsonPropertyName("vendor-id")]
        public int VendorId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("location")]
        public string MapLocation { get; set; }
        [JsonPropertyName("delivery-time")]
        public string DeliveryTime { get; set; }
        [JsonPropertyName("product-image")]
        public string ProductImage { get; set; }
        [JsonPropertyName("product-price")]
        public string ProductPrice { get; set; }
        [JsonPropertyName("promoted")]
        public bool IsPromoted { get; set; }
        [JsonPropertyName("rating")]
        public decimal Rating { get; set; }
        [JsonPropertyName("reviews")]
        public int ReviewCount { get; set; } 
    }
}
