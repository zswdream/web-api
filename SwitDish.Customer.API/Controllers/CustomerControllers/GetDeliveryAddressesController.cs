using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetDeliveryAddressesController : ControllerBase
    {
        private readonly ICustomerDeliveryAddressService customerDeliveryAddressService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public GetDeliveryAddressesController(
            ICustomerDeliveryAddressService customerDeliveryAddressService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerDeliveryAddressService = customerDeliveryAddressService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.GET_DELIVERY_ADDRESSES)]
        [HttpGet]
        public async Task<IActionResult> GetDeliveryAddresses([FromQuery] int customerId)
        {
            try
            {
                var result = await this.customerDeliveryAddressService.GetDeliveryAddressesAsync(customerId).ConfigureAwait(false);
                if (result == null)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.CUSTOMER_NOT_FOUND,
                        Data = null
                    });

                if (result.Count() > 0)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<IEnumerable<CustomerDeliveryAddressViewModel>>(result)
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
    }
}
