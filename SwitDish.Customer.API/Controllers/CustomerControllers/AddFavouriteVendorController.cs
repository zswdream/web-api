using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers.CustomerControllers
{
    [ApiController]
    public class AddFavouriteVendorController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IVendorService vendorService;
        private readonly ICustomerFavouriteVendorService customerFavouriteVendorService;
        private readonly ILoggerManager logger;

        public AddFavouriteVendorController(
            ICustomerService customerService,
            IVendorService vendorService,
            ICustomerFavouriteVendorService customerFavouriteVendorService,
            ILoggerManager logger)
        {
            this.customerService = customerService;
            this.vendorService = vendorService;
            this.customerFavouriteVendorService = customerFavouriteVendorService;
            this.logger = logger;
        }

        [Route(API_REQUESTS.ADD_FAVOURITE_VENDOR)]
        [HttpPost]
        public async Task<IActionResult> AddCustomerFavouriteVendor([FromForm] int customerId, [FromForm] int vendorId)
        {
            try
            {
                var customerExists = await this.customerService.CheckCustomerExistsAsync(customerId).ConfigureAwait(false);
                if (!customerExists)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.CUSTOMER_NOT_FOUND,
                        Data = null
                    });

                var vendorExists = await this.vendorService.CheckVendorExistsAsync(vendorId).ConfigureAwait(false);
                if (!vendorExists)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.VENDOR_NOT_FOUND,
                        Data = null
                    });

                var result = await this.customerFavouriteVendorService.AddVendorAsFavouriteAsync(customerId, vendorId).ConfigureAwait(false);
                if (result == true)
                    return Created(string.Empty, new ApiResponseViewModel
                    {
                        Error = false,
                        Message = API_RESPONSES.FAVOURITE_VENDOR_ADDED,
                        Data = new { customerId, vendorId }
                    });

                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = API_RESPONSES.FAVOURITE_VENDOR_ALREADY_EXISTS,
                    Data = new { customerId, vendorId }
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
