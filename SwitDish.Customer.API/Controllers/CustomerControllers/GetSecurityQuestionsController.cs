using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;
using SwitDish.Customer.API.Models;

namespace SwitDish.Customer.API.Controllers.CustomerControllers
{
    [ApiController]
    public class GetSecurityQuestionsController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerSecurityQuestionService userSecurityQuestionService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public GetSecurityQuestionsController(
            ICustomerService customerService,
            ICustomerSecurityQuestionService userSecurityQuestionService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.userSecurityQuestionService = userSecurityQuestionService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.GET_CUSTOMER_SECURITY_QUESTIONS)]
        [HttpGet]
        public async Task<IActionResult> GetSecurityQuestions([FromQuery] int customerId)
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

                var securityQuestions = await this.userSecurityQuestionService.GetCustomerSecurityQuestionsAsync(customerId).ConfigureAwait(false);

                if (securityQuestions.Count() > 0)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<IEnumerable<CustomerSecurityQuestionViewModel>>(securityQuestions)
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
