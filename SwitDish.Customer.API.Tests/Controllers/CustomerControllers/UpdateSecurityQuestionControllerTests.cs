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
    class UpdateSecurityQuestionControllerTests : IntegrationTestBase
    {
        [TestCase]
        public async Task UpdateSecurityQuestionTest_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "SecurityQuestionId", "1" },
                { "CustomerId", "1" },
                { "Answer", "Dog" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_CUSTOMER_SECURITY_QUESTION, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var securityQuestion = JsonConvert.DeserializeObject<CustomerSecurityQuestionViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Dog", securityQuestion.Answer);
            Assert.IsNotNull(securityQuestion.Question);
        }

        [Test]
        public async Task UpdateSecurityQuestionsTest_FullCircle_Returns_Ok()
        {
            // Arrange
            var customerId = 3;

            // Act - Get Security Questions for Customer and select one to specifically test
            var response_initial = await TestClient.GetAsync($"{API_REQUESTS.GET_CUSTOMER_SECURITY_QUESTIONS}?customerId={customerId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_initial.Content.ReadAsStringAsync().ConfigureAwait(false));
            var existingSecurityQuestions = JsonConvert.DeserializeObject<IEnumerable<CustomerSecurityQuestionViewModel>>(responseData.Data.ToString());
            var existingSecurityQuestionUnderTest = existingSecurityQuestions.FirstOrDefault(d => d.SecurityQuestionId == 3);
            
            // Assert - Answers exist and answer under test is as expected
            Assert.AreEqual(HttpStatusCode.OK, response_initial.StatusCode);
            Assert.IsNotNull(existingSecurityQuestions);
            Assert.AreNotEqual(0, existingSecurityQuestions.Count());
            Assert.IsNotNull(existingSecurityQuestionUnderTest);
            Assert.AreEqual("IDK", existingSecurityQuestionUnderTest.Answer);

            // Arrange
            var data = new Dictionary<string, string>
            {
                { "SecurityQuestionId", "3" },
                { "CustomerId", customerId.ToString() },
                { "Answer", "New Answer" }
            };

            // Act - Update answer for question under test
            var response_update = await TestClient.PatchAsync(API_REQUESTS.UPDATE_CUSTOMER_SECURITY_QUESTION, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_update.Content.ReadAsStringAsync().ConfigureAwait(false));
            var updatedSecurityQuestion = JsonConvert.DeserializeObject<CustomerSecurityQuestionViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response_update.StatusCode);
            Assert.IsNotNull(updatedSecurityQuestion);
            Assert.AreEqual("New Answer", updatedSecurityQuestion.Answer);

            // Act - Get answers for customer again and pick the same specific answer
            var response_final = await TestClient.GetAsync($"{API_REQUESTS.GET_CUSTOMER_SECURITY_QUESTIONS}?customerId={customerId}").ConfigureAwait(false);
            responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_final.Content.ReadAsStringAsync().ConfigureAwait(false));
            var finalSecurityQuestion = JsonConvert.DeserializeObject<IEnumerable<Models.CustomerSecurityQuestionViewModel>>(responseData.Data.ToString());

            // Assert - verify the answer was updated
            Assert.AreEqual(HttpStatusCode.OK, response_initial.StatusCode);
            Assert.IsNotNull(finalSecurityQuestion);
            Assert.AreEqual("New Answer", finalSecurityQuestion.FirstOrDefault(d => d.SecurityQuestionId.Equals(3)).Answer);
        }

        [TestCase]
        public async Task UpdateSecurityQuestionTest_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "SecurityQuestionId", "OH-OH" },
                { "Answer", "Dog" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_CUSTOMER_SECURITY_QUESTION, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
