using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface ICustomerOrderService
    {
        Task<IEnumerable<CustomerOrder>> GetCustomerOrdersAsync(int customerId);
        Task<CustomerOrder> GetCustomerOrderDetailsAsync(int customerOrderId);
        Task<(CustomerOrder, string)> InsertCustomerOrderAsync(CustomerOrder order);
        Task<(CustomerOrder, string)> UpdateCustomerOrderAsync(CustomerOrder order);
        Task<string> CancelCustomerOrderAsync(int customerOrderId);
        Task<string> ValidateCustomerOrderAsync(CustomerOrder order);
    }
}
