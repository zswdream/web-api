using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface IVendorGalleryImageService
    {
        Task<IEnumerable<VendorGalleryImage>> GetVendorGalleryAsync(int vendorId);
        Task<VendorGalleryImage> InsertVendorGalleryAsync(VendorGalleryImage vendorGallery);
        Task<VendorGalleryImage> UpdateVendorGalleryAsync(VendorGalleryImage vendorGallery);
        Task<bool> DeleteVendorGalleryAsync(VendorGalleryImage vendorGallery);
    }
}
