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
    public class CancelOrderController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerOrderService customerOrderService;
        private readonly ILoggerManager logger;

        public CancelOrderController(
            ICustomerService customerService,
            ICustomerOrderService customerOrderService,
            ILoggerManager logger)
        {
            this.customerService = customerService;
            this.customerOrderService = customerOrderService;
            this.logger = logger;
        }

        [Route(API_REQUESTS.CANCEL_ORDER)]
        [HttpPatch]
        public async Task<IActionResult> CancelOrder([FromForm] int CustomerOrderId)
        {
            try
            {
                var result = await this.customerOrderService.CancelCustomerOrderAsync(CustomerOrderId).ConfigureAwait(false);
                if (result == "OK")
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = API_RESPONSES.CUSTOMER_ORDER_CANCELLED,
                        Data = null
                    });

                return BadRequest(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.CUSTOMER_ORDER_NOT_FOUND,
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
