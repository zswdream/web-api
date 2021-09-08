using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Status { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string MapLocation { get; set; }
        public bool DeliversFood { get; set; }
        public bool FreeDelivery { get; set; }
        public string ProfileImage { get; set; }
        public string AvatarImage { get; set; }
        public bool IsPromoted { get; set; }
        public decimal RestaurantCharges { get; set; }
        public decimal DeliveryCharges { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public VendorDeliveryTime VendorDeliveryTime { get; set; }
        public List<Product> Products { get; set; }
        public List<VendorGalleryImage> VendorGalleryImages { get; set; }
        public List<VendorCuisine> VendorCuisines { get; set; }
        public List<VendorOffer> VendorOffers { get; set; }
        public List<VendorServiceCategory> VendorServiceCategories { get; set; }
        public List<VendorFeature> VendorFeatures { get; set; }
        public List<CustomerOrder> CustomerOrders { get; set; }
    }
}
