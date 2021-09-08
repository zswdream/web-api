using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers.CustomerControllers
{
    [ApiController]
    public class GetOrderDetailsController : ControllerBase
    {
        private readonly ICustomerOrderService orderService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public GetOrderDetailsController(
            ICustomerOrderService orderService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.GET_ORDER_DETAILS)]
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails([FromQuery] int customerOrderId)
        {
            try
            {
                var orderDetails = await this.orderService.GetCustomerOrderDetailsAsync(customerOrderId).ConfigureAwait(false);
                if (orderDetails != null)
                {
                    if(orderDetails.Status == DataModel.Enums.OrderStatus.CANCELLED)
                    {
                        return Ok(new ApiResponseViewModel
                        {
                            Error = true,
                            Message = API_RESPONSES.CUSTOMER_ORDER_CANCELLED,
                            Data = null
                        });
                    }

                    return Ok(new ApiResponseViewModel
                        {
                            Error = false,
                            Message = null,
                            Data = this.mapper.Map<CustomerOrderViewModel>(orderDetails)
                        });
                }

                return NotFound(new ApiResponseViewModel
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
