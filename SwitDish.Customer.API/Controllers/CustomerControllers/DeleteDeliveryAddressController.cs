using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers.CustomerControllers
{
    [ApiController]
    public class DeleteDeliveryAddressController : ControllerBase
    {
        private readonly ICustomerDeliveryAddressService customerDeliveryAddressService;
        private readonly ILoggerManager logger;

        public DeleteDeliveryAddressController(
            ICustomerDeliveryAddressService customerDeliveryAddressService,
            ILoggerManager logger)
        {
            this.customerDeliveryAddressService = customerDeliveryAddressService;
            this.logger = logger;
        }

        [Route(API_REQUESTS.DELETE_DELIVERY_ADDRESS)]
        [HttpDelete]
        public async Task<IActionResult> DeleteDeliveryAddress([FromQuery] int customerDeliveryAddressId)
        {
            try
            {
                var result = await this.customerDeliveryAddressService.DeleteDeliveryAddressAsync(customerDeliveryAddressId).ConfigureAwait(false);
                if(result)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = API_RESPONSES.DELIVERY_ADDRESS_DELETED
                    });

                return NotFound(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.DELIVERY_ADDRESS_NOT_FOUND,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
