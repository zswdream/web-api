using AutoMapper;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class VendorOfferService : IVendorOfferService
    {
        private readonly IRepository<VendorOffer> vendorOfferRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public VendorOfferService(
            IRepository<VendorOffer> vendorOfferRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.vendorOfferRepository = vendorOfferRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Common.DomainModels.VendorOffer> GetVendorOfferAsync(int vendorOfferId)
        {
            try
            {
                var vendorOffer = await this.vendorOfferRepository.GetAsync(vendorOfferId).ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.VendorOffer>(vendorOffer);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Common.DomainModels.VendorOffer>> GetVendorOffersAsync(int vendorId)
        {
            try
            {
                var vendorOffers = await this.vendorOfferRepository.GetAsync(filter: d => d.VendorId == vendorId).ConfigureAwait(false);
                return this.mapper.Map<IEnumerable<Common.DomainModels.VendorOffer>>(vendorOffers);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
