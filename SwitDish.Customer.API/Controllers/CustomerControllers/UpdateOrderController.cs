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
    public class UpdateOrderController : ControllerBase
    {
        private readonly ICustomerOrderService customerOrderService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public UpdateOrderController(
            ICustomerOrderService customerOrderService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerOrderService = customerOrderService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.UPDATE_ORDER)]
        [HttpPatch]
        public async Task<IActionResult> UpdateOrder([FromForm] CustomerOrderPostViewModel formData)
        {
            try
            {
                var updatedCustomerOrder = this.mapper.Map<Common.DomainModels.CustomerOrder>(formData);
                var result = await this.customerOrderService.UpdateCustomerOrderAsync(updatedCustomerOrder).ConfigureAwait(false);

                if (result.Item2.Equals("OK"))
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<CustomerOrderViewModel>(result.Item1)
                    });

                return BadRequest(new ApiResponseViewModel
                {
                    Error = true,
                    Message = result.Item2,
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
