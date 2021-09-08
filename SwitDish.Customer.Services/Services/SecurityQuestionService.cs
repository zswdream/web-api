using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly IRepository<SecurityQuestion> securityQuestionRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public SecurityQuestionService(IRepository<SecurityQuestion> securityQuestionRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.securityQuestionRepository = securityQuestionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Common.DomainModels.SecurityQuestion> GetSecurityQuestion(int securityQuestionId)
        {
            try
            {
                var securityQuestion = await this.securityQuestionRepository.GetAsync(securityQuestionId).ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.SecurityQuestion>(securityQuestion);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Common.DomainModels.SecurityQuestion>> GetSecurityQuestions()
        {
            try
            {
                var securityQuestions = await this.securityQuestionRepository.GetAllAsync().ConfigureAwait(false);
                return this.mapper.Map<IEnumerable<Common.DomainModels.SecurityQuestion>>(securityQuestions);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Tuple<Common.DomainModels.SecurityQuestion, string>> InsertSecurityQuestion(string Question)
        {
            try
            {
                var query = await this.securityQuestionRepository.GetAsync(filter: d=> d.Question == Question).ConfigureAwait(false);
                var existingSecurityQuestion = query.FirstOrDefault();
                if (existingSecurityQuestion == null)
                {
                    var newSecurityQuestion = new SecurityQuestion { Question = Question };
                    this.securityQuestionRepository.Insert(newSecurityQuestion);
                    await this.securityQuestionRepository.SaveChangesAsync().ConfigureAwait(false);
                    return Tuple.Create(this.mapper.Map<Common.DomainModels.SecurityQuestion>(newSecurityQuestion), "CREATED");
                }

                return Tuple.Create(this.mapper.Map<Common.DomainModels.SecurityQuestion>(existingSecurityQuestion), "EXISTS");
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
