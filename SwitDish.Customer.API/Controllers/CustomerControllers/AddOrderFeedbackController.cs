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
    public class AddOrderFeedbackController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerOrderService customerOrderService;
        private readonly ICustomerOrderFeedbackService customerOrderFeedbackService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public AddOrderFeedbackController(
            ICustomerService customerService,
            ICustomerOrderService customerOrderService,
            ICustomerOrderFeedbackService customerOrderFeedbackService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.customerOrderService = customerOrderService;
            this.customerOrderFeedbackService = customerOrderFeedbackService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.ADD_ORDER_FEEDBACK)]
        [HttpPost]
        public async Task<IActionResult> PostOrderFeedback([FromForm] CustomerOrderFeedbackViewModel formData)
        {
            try
            {
                var customerExists = await this.customerService.CheckCustomerExistsAsync(formData.CustomerId).ConfigureAwait(false);
                if (!customerExists)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.CUSTOMER_NOT_FOUND,
                        Data = null
                    });

                var customerOrder = await this.customerOrderService.GetCustomerOrderDetailsAsync(formData.CustomerOrderId).ConfigureAwait(false);
                if (customerOrder == null)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.CUSTOMER_ORDER_NOT_FOUND,
                        Data = null
                    });

                if(customerOrder.CustomerId != formData.CustomerId)
                    return BadRequest(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.CUSTOMER_ORDER_NOT_FROM_THIS_CUSTOMER,
                        Data = null
                    });

                if (customerOrder.Status == DataModel.Enums.OrderStatus.CANCELLED)
                    return BadRequest(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.CANCELLED_ORDER_CANNOT_BE_REVIEWED,
                        Data = null
                    });

                var customerOrderFeedback = this.mapper.Map<Common.DomainModels.CustomerOrderFeedback>(formData);
                var result = await this.customerOrderFeedbackService.InsertCustomerOrderFeedbackAsync(customerOrderFeedback).ConfigureAwait(false);
                
                if (result != null)
                    return Created(string.Empty, new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<CustomerOrderFeedbackViewModel>(result)
                    });

                return Problem();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
