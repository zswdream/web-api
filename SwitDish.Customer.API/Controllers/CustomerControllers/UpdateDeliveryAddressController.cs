using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers.CustomerControllers
{
    [ApiController]
    public class UpdateDeliveryAddressController : ControllerBase
    {
        private readonly ICustomerDeliveryAddressService customerDeliveryAddressService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public UpdateDeliveryAddressController(
            ICustomerDeliveryAddressService customerDeliveryAddressService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerDeliveryAddressService = customerDeliveryAddressService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.UPDATE_DELIVERY_ADDRESS)]
        [HttpPatch]
        public async Task<IActionResult> UpdateCustomerDeliveryAddress([FromForm] CustomerDeliveryAddressViewModel formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseViewModel
                {
                    Error = true,
                    Message = null,
                    Data = null
                });

            try
            {
                var customerDeliveryAddress = this.mapper.Map<Common.DomainModels.CustomerDeliveryAddress>(formData);
                var result = await this.customerDeliveryAddressService.UpdateDeliveryAddressAsync(customerDeliveryAddress).ConfigureAwait(false);

                if (result != null)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<CustomerDeliveryAddressViewModel>(result)
                    });

                return NotFound(new ApiResponseViewModel
                {
                    Error = true,
                    Message = null,
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
