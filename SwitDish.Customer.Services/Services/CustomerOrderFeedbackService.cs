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
    public class CustomerOrderFeedbackService : ICustomerOrderFeedbackService
    {
        private readonly IRepository<CustomerOrderFeedback> customerOrderFeedbackRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public CustomerOrderFeedbackService(
            IRepository<CustomerOrderFeedback> customerOrderFeedbackRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.customerOrderFeedbackRepository = customerOrderFeedbackRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Common.DomainModels.CustomerOrderFeedback> GetCustomerOrderFeedbackAsync(int customerOrderFeedbackId)
        {
            try
            {
                var customerOrderFeedback = await this.customerOrderFeedbackRepository.GetAsync(customerOrderFeedbackId).ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.CustomerOrderFeedback>(customerOrderFeedback);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Common.DomainModels.CustomerOrderFeedback>> GetVendorFeedbacksAsync(int vendorId)
        {
            try
            {
                var vendorFeedbacks = await this.customerOrderFeedbackRepository.GetAsync(filter: d => d.CustomerOrder.VendorId == vendorId);
                return this.mapper.Map<IEnumerable<Common.DomainModels.CustomerOrderFeedback>>(vendorFeedbacks);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Common.DomainModels.CustomerOrderFeedback> InsertCustomerOrderFeedbackAsync(Common.DomainModels.CustomerOrderFeedback customerOrderFeedback)
        {
            try
            {
                var customerOrderFeedbackObj = this.mapper.Map<CustomerOrderFeedback>(customerOrderFeedback);
                this.customerOrderFeedbackRepository.Insert(customerOrderFeedbackObj);
                await customerOrderFeedbackRepository.SaveChangesAsync().ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.CustomerOrderFeedback>(customerOrderFeedbackObj);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
