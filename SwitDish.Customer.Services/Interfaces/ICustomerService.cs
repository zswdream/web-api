using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> CheckCustomerExistsAsync(int Id);
        Task<IEnumerable<Common.DomainModels.Customer>> GetAllCustomersAsync();
        Task<Common.DomainModels.Customer> GetCustomerByIdAsync(int Id);
        Task<Common.DomainModels.Customer> GetCustomerByApplicationUserIdAsync(int Id);
        Task<Common.DomainModels.Customer> GetCustomerByEmailAsync(string email);
        Task<Common.DomainModels.Customer> GetCustomerByPhoneAsync(string phone);
        Task<Common.DomainModels.Customer> InsertCustomerAsync(Common.DomainModels.Customer customer);
        Task<Common.DomainModels.Customer> UpdateCustomerAsync(Common.DomainModels.Customer customer);
        Task<Common.DomainModels.Customer> UpdateCustomerProfileAsync(Common.DomainModels.Customer customer);
        Task<bool> DeleteCustomerAsync(Common.DomainModels.Customer customer);
        Task<string> UpdateCustomerProfileImageAsync(int customerId, IFormFile imageFile);
    }
}
