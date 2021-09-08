using AutoMapper;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using System.Threading.Tasks;
using System;

namespace SwitDish.Customer.Services
{
    public class CustomerBookingService : ICustomerBookingService
    {
        private readonly IRepository<DataModel.Models.CustomerBooking> customerBookingRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public CustomerBookingService(
            IRepository<DataModel.Models.CustomerBooking> customerBookingRepository,
            IMapper mapper,
            ILoggerManager logger
            )
        {
            this.customerBookingRepository = customerBookingRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<CustomerBooking> BookCustomerTableAsync(CustomerBooking customerBooking)
        {
            try
            {
                var customerBookingObj = this.mapper.Map<DataModel.Models.CustomerBooking>(customerBooking);
                this.customerBookingRepository.Insert(customerBookingObj);
                await customerBookingRepository.SaveChangesAsync().ConfigureAwait(false);
                return this.mapper.Map<CustomerBooking>(customerBookingObj);
            }
            catch(Exception ex) 
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
