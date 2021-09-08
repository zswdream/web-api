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
    public class AddSecurityQuestionController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerSecurityQuestionService customerSecurityQuestionService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public AddSecurityQuestionController(
            ICustomerService customerService,
            ICustomerSecurityQuestionService customerSecurityQuestionService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.customerSecurityQuestionService = customerSecurityQuestionService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.ADD_CUSTOMER_SECURITY_QUESTION)]
        [HttpPost]
        public async Task<IActionResult> AddSecurityQuestion([FromForm] UserSecurityQuestionViewModel formData)
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

                var result = await this.customerSecurityQuestionService.AddSecurityQuestionAsync(this.mapper.Map<Common.DomainModels.CustomerSecurityQuestion>(formData)).ConfigureAwait(false);
                if(result != null)
                    return Created(string.Empty, new ApiResponseViewModel
                    {
                        Error = false,
                        Message = API_RESPONSES.CUSTOMER_DELIVERY_ADDRESS_ADDED,
                        Data = this.mapper.Map<CustomerSecurityQuestionViewModel>(result)
                    });

                return BadRequest(new ApiResponseViewModel
                {
                    Error = false,
                    Message = API_RESPONSES.CUSTOMER_DELIVERY_ADDRESS_ALREADY_EXISTS,
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
