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
    public class SignupController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public SignupController(
            ICustomerService customerService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.CUSTOMER_SIGNUP)]
        [HttpPost]
        public async Task<IActionResult> Signup([FromForm] SignUpViewModel formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.INVALID_CUSTOMER_DETAILS,
                    Data = null
                });

            if (!formData.Password.Equals(formData.ConfirmPassword))
                return BadRequest(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.NEW_PASSWORDS_DONT_MATCH,
                    Data = null
                });

            try
            {
                var existingEmailCustomer = await this.customerService.GetCustomerByEmailAsync(formData.EmailAddress).ConfigureAwait(false);
                if (existingEmailCustomer != null)
                    return BadRequest(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.EMAIL_ALREADY_REGISTERED,
                        Data = null
                    });

                var existingPhoneCustomer = await this.customerService.GetCustomerByPhoneAsync(formData.MobileNo).ConfigureAwait(false);
                if (existingPhoneCustomer != null)
                    return BadRequest(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.PHONE_ALREADY_REGISTERED,
                        Data = null
                    });

                var customer = this.mapper.Map<Common.DomainModels.Customer>(formData);
                var result = await this.customerService.InsertCustomerAsync(customer);

                if(result != null)
                    return Created(string.Empty, new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<CustomerDetailViewModel>(result)
                    });

                return BadRequest(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.INVALID_CUSTOMER_DETAILS,
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
