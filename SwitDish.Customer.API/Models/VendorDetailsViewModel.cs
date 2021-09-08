using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class VendorDetailsViewModel
    {
        [JsonPropertyName("vendorId")]
        public int VendorId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("brand")]
        public string BrandName { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("primary-phone")]
        public string PrimaryPhone { get; set; }
        [JsonPropertyName("secondary-phone")]
        public string SecondaryPhone { get; set; }
        [JsonPropertyName("primary-email")]
        public string PrimaryEmail { get; set; }
        [JsonPropertyName("secondary-email")]
        public string SecondaryEmail { get; set; }
        [JsonPropertyName("location")]
        public string MapLocation { get; set; }
        [JsonPropertyName("delivers-food")]
        public bool DeliversFood { get; set; }
        [JsonPropertyName("free-delivery")]
        public bool FreeDelivery { get; set; }
        [JsonPropertyName("profile-img")]
        public string ProfileImage { get; set; }
        [JsonPropertyName("avatar-img")]
        public string AvatarImage { get; set; }
        [JsonPropertyName("address")]
        public Address Address { get; set; }
        [JsonPropertyName("delivery-time")]
        public string DeliveryTime { get; set; }
        [JsonPropertyName("restaurant-charges")]
        public decimal RestaurantCharges { get; set; }
        [JsonPropertyName("delivery-charges")]
        public decimal DeliveryCharges { get; set; }
        [JsonPropertyName("rating")]
        public decimal Rating { get; set; }
        [JsonPropertyName("reviews")]
        public int ReviewCount { get; set; }
        [JsonPropertyName("products")]
        public List<object> Products { get; set; }
        [JsonPropertyName("most-popular-products")]
        public List<Product> MostPopularProducts { get; set; }
        [JsonPropertyName("best-seller-products")]
        public List<Product> BestSellerProducts { get; set; }
        [JsonPropertyName("features")]
        public List<VendorFeatureViewModel> VendorFeatures { get; set; }
    }
}
