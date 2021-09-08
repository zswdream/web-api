using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface ICustomerDeliveryAddressService
    {
        Task<CustomerDeliveryAddress> GetDeliveryAddressById(int customerDeliveryAddressId);
        Task<IEnumerable<CustomerDeliveryAddress>> GetDeliveryAddressesAsync(int customerId);
        Task<CustomerDeliveryAddress> InsertDeliveryAddressAsync(CustomerDeliveryAddress customerDeliveryAddress);
        Task<CustomerDeliveryAddress> UpdateDeliveryAddressAsync(CustomerDeliveryAddress customerDeliveryAddress);
        Task<bool> DeleteDeliveryAddressAsync(int customerDeliveryAddressId);
    }
}
