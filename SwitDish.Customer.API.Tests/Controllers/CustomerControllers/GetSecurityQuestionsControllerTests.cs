using Newtonsoft.Json;
using NUnit.Framework;
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
    class GetSecurityQuestionsControllerTests : API.Tests.IntegrationTestBase
    {
        [TestCase(arg: 1, ExpectedResult = 3)]
        public async Task<int> GetSecurityQuestionsTest(int userId)
        {
            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_CUSTOMER_SECURITY_QUESTIONS}?customerId={userId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var SecurityQuestions = JsonConvert.DeserializeObject<IEnumerable<Models.CustomerSecurityQuestionViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return SecurityQuestions.Count();
        }
    }
}
