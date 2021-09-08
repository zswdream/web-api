using SwitDish.Common.DomainModels;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface ICustomerBookingService
    {
        Task<CustomerBooking> BookCustomerTableAsync(CustomerBooking customerBooking);
    }
}
