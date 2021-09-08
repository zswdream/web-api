using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.Interfaces;
using SwitDish.Common.ViewModels;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers
{
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IProductCategoryService productCategoryService;
        private readonly IVendorService vendorService;
        private readonly IVendorGalleryImageService vendorGalleryImageService;
        private readonly IVendorOfferService vendorOfferService;
        private readonly ICustomerOrderFeedbackService customerOrderFeedbackService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public VendorController(
            IProductCategoryService productCategoryService,
            IVendorService vendorService,
            IVendorGalleryImageService vendorGalleryImageService,
            IVendorOfferService vendorOfferService,
            ICustomerOrderFeedbackService customerOrderFeedbackService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.productCategoryService = productCategoryService;
            this.vendorService = vendorService;
            this.vendorGalleryImageService = vendorGalleryImageService;
            this.vendorOfferService = vendorOfferService;
            this.customerOrderFeedbackService = customerOrderFeedbackService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.GET_ALL_VENDORS)]
        [HttpGet]
        public async Task<IActionResult> GetAllVendors([FromQuery] VendorFiltersViewModel filters)
        {
            try
            {
                var allVendors = await this.vendorService.GetAllVendorsAsync(filters).ConfigureAwait(false);
                var vendors = allVendors.Where(d => d.Products.Count() > 0);
                if (vendors.Count() > 0)
                    return Ok(new ApiResponseViewModel 
                        { 
                            Error = false,
                            Message = null,
                            Data = this.mapper.Map<IEnumerable<VendorInfoViewModel>>(vendors)
                        });

                return Ok(new ApiResponseViewModel 
                    {
                        Error = false,
                        Message = null,
                        Data = new object[] { }
                    });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.GET_VENDOR)]
        [HttpGet]
        public async Task<IActionResult> GetVendor(int vendorId)
        {
            try
            {
                var vendor = await this.vendorService.GetVendorByIdAsync(vendorId).ConfigureAwait(false);
                if (vendor != null)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<VendorDetailsViewModel>(vendor)
                    });

                return NotFound(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.VENDOR_NOT_FOUND,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.GET_VENDOR_GALLERY)]
        [HttpGet]
        public async Task<IActionResult> GetVendorGallery(int vendorId)
        {
            try
            {
                var vendorExists = await this.vendorService.CheckVendorExistsAsync(vendorId).ConfigureAwait(false);
                if (!vendorExists)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.VENDOR_NOT_FOUND,
                        Data = null
                    });

                var vendorGallery = await this.vendorGalleryImageService.GetVendorGalleryAsync(vendorId).ConfigureAwait(false);
                if (vendorGallery.Count() > 0)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<IEnumerable<VendorGalleryImageViewModel>>(vendorGallery)
                    });

                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = new object[] { }
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.GET_VENDOR_OFFERS)]
        [HttpGet]
        public async Task<IActionResult> GetVendorOffers(int vendorId)
        {
            try
            {
                var vendorExists = await this.vendorService.CheckVendorExistsAsync(vendorId).ConfigureAwait(false);
                if (!vendorExists)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.VENDOR_NOT_FOUND,
                        Data = null
                    });

                var vendorOffers = await this.vendorOfferService.GetVendorOffersAsync(vendorId).ConfigureAwait(false);
                if (vendorOffers.Count() > 0)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<IEnumerable<VendorOfferViewModel>>(vendorOffers)
                    });

                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = new object[] { }
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.GET_VENDOR_FEEDBACKS)]
        [HttpGet]
        public async Task<IActionResult> GetVendorFeedbacks(int vendorId)
        {
            try
            {
                var vendorExists = await this.vendorService.CheckVendorExistsAsync(vendorId).ConfigureAwait(false);
                if (!vendorExists)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.VENDOR_NOT_FOUND,
                        Data = null
                    });

                var vendorFeedbacks = await this.customerOrderFeedbackService.GetVendorFeedbacksAsync(vendorId).ConfigureAwait(false);
                if (vendorFeedbacks.Count() > 0)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<IEnumerable<VendorFeedbackViewModel>>(vendorFeedbacks)
                    });

                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = new object[] { }
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.GET_VENDOR_FILTERS)]
        [HttpGet]
        public async Task<IActionResult> GetVendorFilters()
        {
            try
            {
                var vendorListFitlers = await this.vendorService.GetVendorListFiltersAsync().ConfigureAwait(false);                
                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = vendorListFitlers
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.GET_PRODUCT_CATEGORIES)]
        [HttpGet]
        public async Task<IActionResult> GetProductCategories()
        {
            try
            {
                var productCategories = await this.productCategoryService.GetProductCategories().ConfigureAwait(false);
                if (productCategories.Count() > 0)
                {
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<IEnumerable<ProductCategoryViewModel>>(productCategories)
                    });
                }

                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = new object[] { }
                });
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
