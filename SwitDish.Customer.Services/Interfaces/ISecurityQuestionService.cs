using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface ISecurityQuestionService
    {
        Task<Tuple<Common.DomainModels.SecurityQuestion, string>> InsertSecurityQuestion(string question);
        Task<SecurityQuestion> GetSecurityQuestion(int securityQuestionId);
        Task<IEnumerable<SecurityQuestion>> GetSecurityQuestions();
    }
}
