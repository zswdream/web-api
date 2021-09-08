using AutoMapper;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class CustomerFavouriteVendorService : ICustomerFavouriteVendorService
    {
        private readonly IRepository<CustomerFavouriteVendor> customerFavouriteVendorRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public CustomerFavouriteVendorService(
            IRepository<CustomerFavouriteVendor> customerFavouriteVendorRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.customerFavouriteVendorRepository = customerFavouriteVendorRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IEnumerable<Common.DomainModels.CustomerFavouriteVendor>> GetCustomerFavouriteVendorsAsync(int customerId)
        {
            try
            {
                var customerFavouriteVendors = await this.customerFavouriteVendorRepository.GetAsync(filter: d => d.CustomerId == customerId).ConfigureAwait(false);
                return this.mapper.Map<IEnumerable<Common.DomainModels.CustomerFavouriteVendor>>(customerFavouriteVendors);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<bool> AddVendorAsFavouriteAsync(int customerId, int vendorId)
        {
            try
            {
                var query = await this.customerFavouriteVendorRepository.GetAsync(filter: d => d.CustomerId.Equals(customerId) && d.VendorId.Equals(vendorId)).ConfigureAwait(false);
                var existingFlag = query.SingleOrDefault();

                if (existingFlag == null)
                {
                    var newCustomerFavouriteVendor = new CustomerFavouriteVendor { CustomerId = customerId, VendorId = vendorId };
                    this.customerFavouriteVendorRepository.Insert(newCustomerFavouriteVendor);
                    await this.customerFavouriteVendorRepository.SaveChangesAsync().ConfigureAwait(false);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return false;
            }
        }
        public async Task<bool> RemoveVendorAsFavouriteAsync(int customerId, int vendorId)
        {
            try
            {
                var query = await this.customerFavouriteVendorRepository.GetAsync(filter: d => d.CustomerId.Equals(customerId) && d.VendorId.Equals(vendorId)).ConfigureAwait(false);
                var existingFlag = query.SingleOrDefault();

                if (existingFlag != null)
                {
                    this.customerFavouriteVendorRepository.Delete(existingFlag);
                    await this.customerFavouriteVendorRepository.SaveChangesAsync().ConfigureAwait(false);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
