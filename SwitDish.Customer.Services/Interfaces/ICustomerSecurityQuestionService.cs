using SwitDish.Common.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface ICustomerSecurityQuestionService
    {
        Task<IEnumerable<CustomerSecurityQuestion>> GetCustomerSecurityQuestionsAsync(int customerId);
        Task<CustomerSecurityQuestion> AddSecurityQuestionAsync(CustomerSecurityQuestion data);
        Task<CustomerSecurityQuestion> UpdateSecurityQuestionAsync(CustomerSecurityQuestion data);
    }
}
