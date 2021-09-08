using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers.CustomerControllers
{
    [ApiController]
    public class DeleteFavouriteVendorController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IVendorService vendorService;
        private readonly ICustomerFavouriteVendorService customerFavouriteVendorService;
        private readonly ILoggerManager logger;

        public DeleteFavouriteVendorController(
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

        [Route(API_REQUESTS.DELETE_FAVOURITE_VENDOR)]
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerFavouriteVendor([FromQuery] int customerId, [FromQuery] int vendorId)
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

                var result = await this.customerFavouriteVendorService.RemoveVendorAsFavouriteAsync(customerId, vendorId).ConfigureAwait(false);
                if (result == true)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = API_RESPONSES.FAVOURITE_VENDOR_REMOVED,
                        Data = null
                    });

                return NotFound(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.FAVOURITE_VENDOR_NOT_FOUND,
                    Data = null
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
