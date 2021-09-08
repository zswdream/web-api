using AutoMapper;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class CustomerSecurityQuestionService : ICustomerSecurityQuestionService
    {
        private readonly IRepository<DataModel.Models.CustomerSecurityQuestion> customerSecurityQuestionRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public CustomerSecurityQuestionService(
            IRepository<DataModel.Models.CustomerSecurityQuestion> customerSecurityQuestionRepository,
            IMapper mapper,
            ILoggerManager logger)

        {
            this.customerSecurityQuestionRepository = customerSecurityQuestionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<CustomerSecurityQuestion>> GetCustomerSecurityQuestionsAsync(int customerId)
        {
            try
            {
                var securityQuestions = await this.customerSecurityQuestionRepository.GetAsync(filter: d => d.CustomerId == customerId).ConfigureAwait(false);
                return this.mapper.Map<IEnumerable<CustomerSecurityQuestion>>(securityQuestions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<CustomerSecurityQuestion> AddSecurityQuestionAsync(CustomerSecurityQuestion data)
        {
            try
            {
                var query = await this.customerSecurityQuestionRepository.GetAsync(filter: d => d.SecurityQuestionId == data.SecurityQuestionId && d.CustomerId == data.CustomerId).ConfigureAwait(false);
                var existingRecord = query.SingleOrDefault();

                if (existingRecord == null)
                {
                    var newRecord = this.mapper.Map<DataModel.Models.CustomerSecurityQuestion>(data);
                    this.customerSecurityQuestionRepository.Insert(newRecord);
                    await this.customerSecurityQuestionRepository.SaveChangesAsync().ConfigureAwait(false);

                    return this.mapper.Map<CustomerSecurityQuestion>(newRecord);
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<CustomerSecurityQuestion> UpdateSecurityQuestionAsync(CustomerSecurityQuestion data)
        {
            try
            {
                var query = await this.customerSecurityQuestionRepository.GetAsync(filter: d => d.SecurityQuestionId == data.SecurityQuestionId && d.CustomerId == data.CustomerId, tracking: true).ConfigureAwait(false);
                var existingRecord = query.SingleOrDefault();
                
                if (existingRecord != null)
                {
                    existingRecord.Answer = data.Answer;
                    await this.customerSecurityQuestionRepository.SaveChangesAsync().ConfigureAwait(false);
                    return this.mapper.Map<CustomerSecurityQuestion>(existingRecord);
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
