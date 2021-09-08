using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers
{
    [ApiController]
    public class SecurityQuestionController : ControllerBase
    {
        private readonly ISecurityQuestionService securityQuestionService;
        private readonly ILoggerManager logger;
        public SecurityQuestionController(
            ISecurityQuestionService securityQuestionService,
            ILoggerManager logger)
        {
            this.securityQuestionService = securityQuestionService;
            this.logger = logger;
        }

        [Route(API_REQUESTS.ADD_SECURITY_QUESTION)]
        [HttpPost]
        public async Task<IActionResult> AddSecurityQuestion([FromForm] string Question)
        {
            try
            {
                var result = await this.securityQuestionService.InsertSecurityQuestion(Question).ConfigureAwait(false);

                if(result != null)
                    return result.Item2 switch {
                        "CREATED" => Created(string.Empty, new ApiResponseViewModel
                        {
                            Error = false,
                            Message = API_RESPONSES.SECURITY_QUESTION_ADDED,
                            Data = result.Item1
                        }),
                        _ => Ok(new ApiResponseViewModel
                        {
                            Error = false,
                            Message = API_RESPONSES.SECURITY_QUESTION_UPDATED,
                            Data = result.Item1
                        })
                    };

                return Problem();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.GET_SECURITY_QUESTION)]
        [HttpGet]
        public async Task<IActionResult> GetSecurityQuestion([FromQuery] int SecurityQuestionId)
        {
            try
            {
                var result = await this.securityQuestionService.GetSecurityQuestion(SecurityQuestionId).ConfigureAwait(false);
                if(result != null)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = result
                    });

                return NotFound(new ApiResponseViewModel
                {
                    Error = true,
                    Message = API_RESPONSES.SECURITY_QUESTION_NOT_FOUND,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.GET_SECURITY_QUESTIONS)]
        [HttpGet]
        public async Task<IActionResult> GetSecurityQuestions()
        {
            try
            {
                var securityQuestions = await this.securityQuestionService.GetSecurityQuestions().ConfigureAwait(false);

                if (securityQuestions.Count() > 0)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = securityQuestions
                    });

                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = new object[] { }
                }
);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
