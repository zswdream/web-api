using AutoMapper;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class VendorFeedbackReactionService : IVendorFeedbackReactionService
    {
        private readonly IRepository<DataModel.Models.VendorFeedbackReaction> vendorFeedbackReactionRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public VendorFeedbackReactionService(
            IRepository<DataModel.Models.VendorFeedbackReaction> vendorFeedbackReactionRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.vendorFeedbackReactionRepository = vendorFeedbackReactionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<VendorFeedbackReaction>> GetVendorFeedbackReactionsAsync(int vendorFeedbackId)
        {
            try
            {
                var vendorFeedbackReactions = await this.vendorFeedbackReactionRepository.GetAsync(filter: d => d.CustomerOrderFeedbackId.Equals(vendorFeedbackId)).ConfigureAwait(false);
                return this.mapper.Map<IEnumerable<VendorFeedbackReaction>>(vendorFeedbackReactions);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteVendorFeedbackReactionAsync(VendorFeedbackReaction vendorFeedbackReaction)
        {
            try
            {
                var query = await this.vendorFeedbackReactionRepository.GetAsync(
                    filter: d=> d.CustomerOrderFeedbackId == vendorFeedbackReaction.CustomerOrderFeedbackId 
                    && d.CustomerId == vendorFeedbackReaction.CustomerId 
                    && d.ReactionType == vendorFeedbackReaction.ReactionType);

                if (query.Count() == 0)
                    return false;

                var vendorFeedbackReactionToDelete = query.FirstOrDefault();
                this.vendorFeedbackReactionRepository.Delete(vendorFeedbackReactionToDelete);
                await vendorFeedbackReactionRepository.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<VendorFeedbackReaction> InsertVendorFeedbackReactionAsync(VendorFeedbackReaction vendorFeedbackReaction)
        {
            try
            {
                var query = await this.vendorFeedbackReactionRepository.GetAsync(
                    filter: d => d.CustomerOrderFeedbackId == vendorFeedbackReaction.CustomerOrderFeedbackId
                    && d.CustomerId == vendorFeedbackReaction.CustomerId);

                var existingReaction = query.FirstOrDefault();
                
                // If reaction already exists, update it
                if(existingReaction != null)
                {
                    if(existingReaction.ReactionType != vendorFeedbackReaction.ReactionType)
                    {
                        existingReaction.ReactionType = vendorFeedbackReaction.ReactionType;
                        await vendorFeedbackReactionRepository.SaveChangesAsync().ConfigureAwait(false);
                    }

                    return this.mapper.Map<VendorFeedbackReaction>(existingReaction);
                }

                var vendorFeedbackReactionObj = this.mapper.Map<DataModel.Models.VendorFeedbackReaction>(vendorFeedbackReaction);
                vendorFeedbackReactionRepository.Insert(vendorFeedbackReactionObj);
                await vendorFeedbackReactionRepository.SaveChangesAsync().ConfigureAwait(false);
                return this.mapper.Map<VendorFeedbackReaction>(vendorFeedbackReactionObj);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
