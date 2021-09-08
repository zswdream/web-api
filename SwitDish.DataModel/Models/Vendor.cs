using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class Vendor
    {
        public Vendor()
        {
            Products = new HashSet<Product>();
            VendorGalleryImages = new HashSet<VendorGalleryImage>();
            VendorCuisines = new HashSet<VendorCuisine>();
            VendorOffers = new HashSet<VendorOffer>();
            VendorServiceCategories = new HashSet<VendorServiceCategory>();
            VendorFeatures = new HashSet<VendorFeature>();
            CustomerOrders = new HashSet<CustomerOrder>();
        }
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
        public virtual Address Address { get; set; }
        public virtual VendorDeliveryTime VendorDeliveryTime { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<VendorGalleryImage> VendorGalleryImages { get; set; }
        public virtual ICollection<VendorCuisine> VendorCuisines { get; set; }
        public virtual ICollection<VendorOffer> VendorOffers { get; set; }
        public virtual ICollection<VendorServiceCategory> VendorServiceCategories { get; set; }
        public virtual ICollection<VendorFeature> VendorFeatures { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
    }
}
