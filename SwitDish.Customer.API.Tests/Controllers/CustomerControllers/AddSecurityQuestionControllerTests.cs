using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Tests;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture()]
    class AddSecurityQuestionControllerTests : IntegrationTestBase
    {
        [TestCase]
        public async Task AddSecurityQuestionTest_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "SecurityQuestionId", "3" },
                { "CustomerId", "1" },
                { "Answer", "ABC" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_CUSTOMER_SECURITY_QUESTION, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var securityQuestion = JsonConvert.DeserializeObject<CustomerSecurityQuestionViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("ABC", securityQuestion.Answer);
            //Assert.IsNotNull(securityQuestion.Question);
        }

        [TestCase]
        public async Task AddSecurityQuestionTest_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "SecurityQuestionId", "2" },
                { "CustomerId", "1" },
                { "Answer", "ABC" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_CUSTOMER_SECURITY_QUESTION, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
