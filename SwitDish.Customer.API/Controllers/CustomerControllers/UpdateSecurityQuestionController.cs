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
    public class UpdateSecurityQuestionController : ControllerBase
    {
        private readonly ICustomerSecurityQuestionService customerSecurityQuestionService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public UpdateSecurityQuestionController(
            ICustomerSecurityQuestionService customerSecurityQuestionService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerSecurityQuestionService = customerSecurityQuestionService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.UPDATE_CUSTOMER_SECURITY_QUESTION)]
        [HttpPatch]
        public async Task<IActionResult> UpdateSecurityQuestion([FromForm] UserSecurityQuestionViewModel formData)
        {
            try
            {
                var result = await this.customerSecurityQuestionService.UpdateSecurityQuestionAsync(this.mapper.Map<Common.DomainModels.CustomerSecurityQuestion>(formData)).ConfigureAwait(false);
                if(result != null)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<CustomerSecurityQuestionViewModel>(result)
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
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
