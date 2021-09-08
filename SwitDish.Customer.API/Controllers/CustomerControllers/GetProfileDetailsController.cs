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
    public class GetProfileDetailsController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public GetProfileDetailsController(
            ICustomerService customerService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.GET_PROFILE_DETAILS)]
        [HttpGet]
        public async Task<IActionResult> GetProfileDetails([FromQuery] int applicationUserId, [FromQuery] int customerId)
        {
            try
            {
                Common.DomainModels.Customer customer;

                if (applicationUserId != 0)
                    customer = await this.customerService.GetCustomerByApplicationUserIdAsync(applicationUserId).ConfigureAwait(false);
                else if (customerId != 0)
                    customer = await this.customerService.GetCustomerByIdAsync(customerId).ConfigureAwait(false);
                else customer = null;
                
                if (customer != null)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<CustomerDetailViewModel>(customer)
                    });
                
                return NotFound(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.CUSTOMER_NOT_FOUND,
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
