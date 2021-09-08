using AutoMapper;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class CustomerDeliveryAddressService : ICustomerDeliveryAddressService
    {
        private readonly IRepository<DataModel.Models.CustomerDeliveryAddress> customerDeliveryAddressRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        private readonly ICustomerService customerService;

        public CustomerDeliveryAddressService(
            IRepository<DataModel.Models.CustomerDeliveryAddress> customerDeliveryAddressRepository,
            IMapper mapper,
            ILoggerManager logger,
            ICustomerService customerService)
        {
            this.customerDeliveryAddressRepository = customerDeliveryAddressRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.customerService = customerService;
        }
        public async Task<bool> DeleteDeliveryAddressAsync(int customerDeliveryAddressId)
        {
            try
            {
                var customerDeliveryAddressObj = await this.customerDeliveryAddressRepository.GetAsync(customerDeliveryAddressId);
                if (customerDeliveryAddressObj == null)
                    return false;

                customerDeliveryAddressObj.IsDeleted = true;
                await customerDeliveryAddressRepository.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<CustomerDeliveryAddress>> GetDeliveryAddressesAsync(int customerId)
        {
            try
            {
                var existingCustomer = await this.customerService.GetCustomerByIdAsync(customerId);
                if (existingCustomer == null)
                    return null;

                var customerDeliveryAddresses = await this.customerDeliveryAddressRepository.GetAsync(filter: d => d.CustomerId.Equals(customerId) && !d.IsDeleted).ConfigureAwait(false);
                return this.mapper.Map<IEnumerable<CustomerDeliveryAddress>>(customerDeliveryAddresses);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<CustomerDeliveryAddress> InsertDeliveryAddressAsync(CustomerDeliveryAddress customerDeliveryAddress)
        {
            try
            {
                var customerDeliveryAddressObj = this.mapper.Map<DataModel.Models.CustomerDeliveryAddress>(customerDeliveryAddress);
                customerDeliveryAddressRepository.Insert(customerDeliveryAddressObj);
                await customerDeliveryAddressRepository.SaveChangesAsync().ConfigureAwait(false);
                return this.mapper.Map<CustomerDeliveryAddress>(customerDeliveryAddressObj);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<CustomerDeliveryAddress> UpdateDeliveryAddressAsync(CustomerDeliveryAddress customerDeliveryAddress)
        {
            try
            {
                var query = await this.customerDeliveryAddressRepository.GetAsync(d => d.CustomerDeliveryAddressId == customerDeliveryAddress.CustomerDeliveryAddressId && d.CustomerId == customerDeliveryAddress.CustomerId).ConfigureAwait(false);
                var existingRecord = query.FirstOrDefault();
                if (existingRecord != null && !existingRecord.IsDeleted)
                {
                    existingRecord.CompleteAddress = customerDeliveryAddress.CompleteAddress;
                    existingRecord.DeliveryArea = customerDeliveryAddress.DeliveryArea;
                    existingRecord.Instructions = customerDeliveryAddress.Instructions;
                    existingRecord.CustomerDeliveryAddressType = customerDeliveryAddress.CustomerDeliveryAddressType;

                    customerDeliveryAddressRepository.Update(existingRecord);
                    await customerDeliveryAddressRepository.SaveChangesAsync().ConfigureAwait(false);
                    return this.mapper.Map<CustomerDeliveryAddress>(existingRecord);
                }
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
            return null;
        }
        
        public async Task<CustomerDeliveryAddress> GetDeliveryAddressById(int customerDeliveryAddressId)
        {
            try
            {
                var customerDeliveryAddress = await this.customerDeliveryAddressRepository.GetAsync(customerDeliveryAddressId).ConfigureAwait(false);
                if(customerDeliveryAddress != null && !customerDeliveryAddress.IsDeleted)
                    return this.mapper.Map<CustomerDeliveryAddress>(customerDeliveryAddress);

                return null;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
