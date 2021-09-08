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
    public class UpdateProfileDetailsController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public UpdateProfileDetailsController(
            ICustomerService customerService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.UPDATE_PROFILE_DETAILS)]
        [HttpPatch]
        public async Task<IActionResult> UpdateProfileDetails([FromForm] CustomerProfileUpdateViewModel formData)
        {
            try
            {
                var existingEmailCustomer = await this.customerService.GetCustomerByEmailAsync(formData.Email).ConfigureAwait(false);
                if (existingEmailCustomer != null && existingEmailCustomer.CustomerId != formData.CustomerId)
                    return BadRequest(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.EMAIL_ALREADY_REGISTERED,
                        Data = null
                    });

                var existingPhoneCustomer = await this.customerService.GetCustomerByPhoneAsync(formData.Phone).ConfigureAwait(false);
                if (existingPhoneCustomer != null && existingPhoneCustomer.CustomerId != formData.CustomerId)
                    return BadRequest(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.PHONE_ALREADY_REGISTERED,
                        Data = null
                    });

                var result = await this.customerService.UpdateCustomerProfileAsync(this.mapper.Map<Common.DomainModels.Customer>(formData)).ConfigureAwait(false);

                if (result == null)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = null,
                        Data = null
                    });

                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = this.mapper.Map<CustomerDetailViewModel>(result)
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
