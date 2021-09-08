using System.Collections.Generic;
using System.Threading.Tasks;
using SwitDish.Common.DomainModels;
using SwitDish.Common.ViewModels;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface IVendorService
    {
        Task<bool> CheckVendorExistsAsync(int Id);
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();
        Task<IEnumerable<Vendor>> GetAllVendorsAsync(VendorFiltersViewModel filters);
        Task<Vendor> GetVendorByIdAsync(int Id);
        Task<VendorListFiltersViewModel> GetVendorListFiltersAsync();
    }
}
