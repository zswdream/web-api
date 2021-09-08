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
    public class AddVendorFeedbackReactionController : ControllerBase
    {
        private readonly IVendorFeedbackReactionService vendorFeedbackReactionService;
        private readonly ICustomerOrderFeedbackService customerOrderFeedbackService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public AddVendorFeedbackReactionController(
            IVendorFeedbackReactionService vendorFeedbackReactionService,
            ICustomerOrderFeedbackService customerOrderFeedbackService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.vendorFeedbackReactionService = vendorFeedbackReactionService;
            this.customerOrderFeedbackService = customerOrderFeedbackService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.ADD_VENDOR_FEEDBACK_REACTION)]
        [HttpPost]
        public async Task<IActionResult> AddVendorFeedbackReaction([FromForm] VendorFeedbackReactionViewModel formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseViewModel
                {
                    Error = true,
                    Message = null,
                    Data = null
                });

            try
            {
                var vendorFeedback = await this.customerOrderFeedbackService.GetCustomerOrderFeedbackAsync(formData.CustomerOrderFeedbackId);
                if (vendorFeedback == null)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.FEEDBACK_NOT_FOUND,
                        Data = null
                    });

                var vendorFeedbackReaction = this.mapper.Map<Common.DomainModels.VendorFeedbackReaction>(formData);
                var result = await this.vendorFeedbackReactionService.InsertVendorFeedbackReactionAsync(vendorFeedbackReaction).ConfigureAwait(false);

                if (result != null)
                    return Created(string.Empty, new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<VendorFeedbackReactionViewModel>(result)
                    });
                
                return Problem();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
