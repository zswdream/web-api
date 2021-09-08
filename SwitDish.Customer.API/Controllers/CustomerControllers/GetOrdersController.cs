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
    public class GetOrdersController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerOrderService customerOrderService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public GetOrdersController(
            ICustomerService customerService,
            ICustomerOrderService customerOrderService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.customerOrderService = customerOrderService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.GET_ORDERS)]
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] int customerId)
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

                var orders = await this.customerOrderService.GetCustomerOrdersAsync(customerId).ConfigureAwait(false);
                if (orders.Count() > 0)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<IEnumerable<CustomerOrderViewModel>>(orders)
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
