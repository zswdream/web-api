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
    public class VendorGalleryImageService : IVendorGalleryImageService
    {
        private readonly IRepository<VendorGalleryImage> vendorGalleryImageRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public VendorGalleryImageService(
            IRepository<VendorGalleryImage> vendorGalleryImageRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.vendorGalleryImageRepository = vendorGalleryImageRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<bool> DeleteVendorGalleryAsync(Common.DomainModels.VendorGalleryImage vendorGalleryImage)
        {
            try
            {
                var vendorGalleryObj = this.mapper.Map<VendorGalleryImage>(vendorGalleryImage);
                this.vendorGalleryImageRepository.Delete(vendorGalleryObj);
                await vendorGalleryImageRepository.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Common.DomainModels.VendorGalleryImage>> GetVendorGalleryAsync(int vendorId)
        {
            try
            {
                var vendorGallery = await this.vendorGalleryImageRepository.GetAsync(filter: d => d.VendorId.Equals(vendorId)).ConfigureAwait(false);
                return this.mapper.Map<IEnumerable<Common.DomainModels.VendorGalleryImage>>(vendorGallery);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Common.DomainModels.VendorGalleryImage> InsertVendorGalleryAsync(Common.DomainModels.VendorGalleryImage vendorGalleryImage)
        {
            try
            {
                var vendorGalleryObj = this.mapper.Map<VendorGalleryImage>(vendorGalleryImage);
                vendorGalleryImageRepository.Insert(vendorGalleryObj);
                await vendorGalleryImageRepository.SaveChangesAsync().ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.VendorGalleryImage>(vendorGalleryObj);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Common.DomainModels.VendorGalleryImage> UpdateVendorGalleryAsync(Common.DomainModels.VendorGalleryImage vendorGalleryImage)
        {
            try
            {
                var vendorGalleryObj = this.mapper.Map<VendorGalleryImage>(vendorGalleryImage);
                vendorGalleryImageRepository.Update(vendorGalleryObj);
                await vendorGalleryImageRepository.SaveChangesAsync().ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.VendorGalleryImage>(vendorGalleryObj);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
