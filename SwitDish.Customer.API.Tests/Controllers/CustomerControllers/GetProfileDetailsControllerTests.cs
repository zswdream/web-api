using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture()]
    class GetProfileDetailsControllerTests : API.Tests.IntegrationTestBase
    {
        [TestCase(arg: 1)]
        public async Task GetProfileDetailsTest(int applicationUserId)
        {
            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_PROFILE_DETAILS}?applicationUserId={applicationUserId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var customerDetails = JsonConvert.DeserializeObject<CustomerDetailViewModel>(responseData.Data.ToString());

            // Assert
            Assert.IsNotNull(customerDetails);
            Assert.AreEqual("Some", customerDetails.FirstName);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
