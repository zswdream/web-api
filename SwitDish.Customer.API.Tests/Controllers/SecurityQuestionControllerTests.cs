using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Common.DomainModels;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.Tests
{
    [TestFixture]
    public class SecurityQuestionControllerTests : API.Tests.IntegrationTestBase
    {
        [Test]
        public async Task AddSecurityQuestionTest_Success()
        {
            // Arrange
            var testQuestion = "How are you?";
            var data = new Dictionary<string, string>
            {
                { "Question", testQuestion }
            };

            // Act
            var response = await TestClient.PostAsync($"{API_REQUESTS.ADD_SECURITY_QUESTION}", new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var securityQuestion = JsonConvert.DeserializeObject<SecurityQuestion>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(securityQuestion);
            Assert.AreEqual(testQuestion, securityQuestion.Question);
        }

        [Test]
        public async Task GetSecurityQuestionTest_Found()
        {
            // Arrange
            var testSecurityQuestionId = 1;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_SECURITY_QUESTION}?securityQuestionId={testSecurityQuestionId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var securityQuestion = JsonConvert.DeserializeObject<SecurityQuestion>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(securityQuestion);
        }

        [Test]
        public async Task GetSecurityQuestionTest_NotFound()
        {
            // Arrange
            var testSecurityQuestionId = 9999;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_SECURITY_QUESTION}?securityQuestionId={testSecurityQuestionId}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetSecurityQuestionsTest()
        {
            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_SECURITY_QUESTIONS}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var securityQuestions = JsonConvert.DeserializeObject<IEnumerable<SecurityQuestion>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(securityQuestions);
            Assert.AreNotEqual(0, securityQuestions.Count());
        }
    }
}