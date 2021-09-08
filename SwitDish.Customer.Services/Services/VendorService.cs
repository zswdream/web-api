using AutoMapper;
using SwitDish.Common.Interfaces;
using SwitDish.Common.ViewModels;
using SwitDish.Customer.Services.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class VendorService : IVendorService
    {
        private readonly IRepository<Vendor> vendorRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public VendorService(
            IRepository<Vendor> vendorRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.vendorRepository = vendorRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<bool> CheckVendorExistsAsync(int Id)
        {
            try
            {
                var query = await this.vendorRepository.GetAsync(filter: d=> d.VendorId == Id, tracking: false).ConfigureAwait(false);
                return query.Count() > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return false;
            }
        }
        public async Task<IEnumerable<Common.DomainModels.Vendor>> GetAllVendorsAsync()
        {
            try
            {
                var vendors = await this.vendorRepository.GetAllAsync().ConfigureAwait(false);

                // filter vendors with atleast one product
                vendors = vendors.Where(d => d.Products.Any());

                return this.mapper.Map<IEnumerable<Common.DomainModels.Vendor>>(vendors);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<Common.DomainModels.Vendor> GetVendorByIdAsync(int Id)
        {
            try
            {
                var vendor = await this.vendorRepository.GetAsync(Id).ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.Vendor>(vendor);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<Common.DomainModels.Vendor>> GetAllVendorsAsync(VendorFiltersViewModel filters)
        {
            try
            {
                var vendors = await this.GetAllVendorsAsync().ConfigureAwait(false);
                
                if(filters.Locations != null)
                    vendors = vendors.Where(d => filters.Locations.Contains(d.Address.City));

                if (filters.Cuisines != null)
                    vendors = vendors.Where(d => filters.Cuisines.Any(e=> d.VendorCuisines.Select(f=> f.Cuisine.Name).Contains(e)));
                
                if (filters.Features != null)
                    vendors = vendors.Where(d => filters.Features.Any(e=> d.VendorFeatures.Select(f=> f.Feature.Name).Contains(e)));
                
                if (filters.DeliveryMinutes != null)
                {
                    foreach (var deliveryMinutes in filters.DeliveryMinutes)
                    {
                        if(deliveryMinutes != null)
                            vendors = vendors.Where(d => 
                            d.VendorDeliveryTime != null 
                            && d.VendorDeliveryTime.Minimum.TotalMinutes <= deliveryMinutes
                            && d.VendorDeliveryTime.Maximum.TotalMinutes >= deliveryMinutes);
                    }
                }
            
                if (filters.Categories != null)
                    vendors = vendors.Where(d => filters.Categories.Any(e=> d.VendorServiceCategories.Select(f=> f.ServiceCategory.Name).Contains(e)));

                if (filters.ProductCategories != null)
                    vendors = vendors.Where(d => filters.ProductCategories.Any(e => d.Products.Any(f => f.ProductCategory.Name.Equals(e))));

                return this.mapper.Map<IEnumerable<Common.DomainModels.Vendor>>(vendors);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<VendorListFiltersViewModel> GetVendorListFiltersAsync()
        {
            try
            {
                var vendors = await this.GetAllVendorsAsync().ConfigureAwait(false);
                
                var vendorListFilters = new VendorListFiltersViewModel();
                vendorListFilters.TotalVendors = vendors.Count();

                var vendorLocations = vendors.Where(d=> d.Address != null).Select(d => d.Address.City).Distinct();
                foreach (var location in vendorLocations)
                {
                    vendorListFilters.Locations.Add(new FilterDetailViewModel
                    {
                        name = location,
                        count = vendors.Where(d=> d.Address.City == location).Count()
                    });
                }

                var vendorCuisines = vendors.SelectMany(d => d.VendorCuisines.Select(e=> e.Cuisine.Name)).Distinct();
                foreach (var cuisine in vendorCuisines)
                {
                    vendorListFilters.Cuisines.Add(new FilterDetailViewModel
                    {
                        name = cuisine,
                        count = vendors.Where(d => d.VendorCuisines.Any(e => e.Cuisine.Name == cuisine)).Count()
                    });
                }

                var vendorFeatures = vendors.SelectMany(d => d.VendorFeatures.Select(e => e.Feature.Name)).Distinct();
                foreach (var feature in vendorFeatures)
                {
                    vendorListFilters.Features.Add(new FilterDetailViewModel
                    {
                        name = feature,
                        count = vendors.Where(d => d.VendorFeatures.Any(e => e.Feature.Name == feature)).Count()
                    });
                }

                var vendorDeliveryTimes = vendors.Where(d=> d.VendorDeliveryTime != null).Select(d => d.VendorDeliveryTime.ToString()).Distinct();
                foreach (var deliveryTime in vendorDeliveryTimes)
                {
                    vendorListFilters.DeliveryMinutes.Add(new FilterDetailViewModel
                    {
                        name = deliveryTime,
                        count = vendors.Where(d =>
                            d.VendorDeliveryTime != null 
                            && d.VendorDeliveryTime.ToString() == deliveryTime).Count()
                    });
                }

                var vendorServiceCategories = vendors.SelectMany(d => d.VendorServiceCategories.Select(e => e.ServiceCategory.Name)).Distinct();
                foreach (var serviceCategory in vendorServiceCategories)
                {
                    vendorListFilters.Categories.Add(new FilterDetailViewModel
                    {
                        name = serviceCategory,
                        count = vendors.Where(d => d.VendorServiceCategories.Any(e => e.ServiceCategory.Name == serviceCategory)).Count()
                    });
                }

                return vendorListFilters;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
