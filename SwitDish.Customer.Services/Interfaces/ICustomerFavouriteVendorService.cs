using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface ICustomerFavouriteVendorService
    {
        Task<IEnumerable<Common.DomainModels.CustomerFavouriteVendor>> GetCustomerFavouriteVendorsAsync(int customerId);
        Task<bool> AddVendorAsFavouriteAsync(int customerId, int vendorId);
        Task<bool> RemoveVendorAsFavouriteAsync(int customerId, int vendorId);
    }
}
