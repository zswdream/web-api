using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface ICustomerOrderFeedbackService
    {
        Task<CustomerOrderFeedback> GetCustomerOrderFeedbackAsync(int customerOrderFeedbackId);
        Task<CustomerOrderFeedback> InsertCustomerOrderFeedbackAsync(CustomerOrderFeedback customerOrderFeedback);
        Task<IEnumerable<CustomerOrderFeedback>> GetVendorFeedbacksAsync(int vendorId);
    }
}
