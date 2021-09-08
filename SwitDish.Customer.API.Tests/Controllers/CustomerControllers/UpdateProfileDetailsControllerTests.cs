using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture()]
    class UpdateProfileDetailsControllerTests : API.Tests.IntegrationTestBase
    {
        [Test]
        public async Task UpdateProfileDetailsTest_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "FirstName", "test" },
                { "LastName", "user" },
                { "Email", "test@test23.com" },
                { "Phone", "12345672323" },
                { "DateOfBirth", "01-01-2000" }
            };
            
            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_PROFILE_DETAILS, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var customer = JsonConvert.DeserializeObject<CustomerDetailViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual("test@test23.com", customer.Email);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetProfileDetailsTest_Returns_Ok()
        {
            // Arrange
            var testApplicationUserId = 1;

            // Act - For Initial Customer Profile data
            var response_initial = await TestClient.GetAsync($"{API_REQUESTS.GET_PROFILE_DETAILS}?applicationUserId={testApplicationUserId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_initial.Content.ReadAsStringAsync().ConfigureAwait(false));
            var initialCustomerDetails = JsonConvert.DeserializeObject<CustomerDetailViewModel>(responseData.Data.ToString());

            // Assert - For Initial Customer Profile data
            Assert.AreEqual(HttpStatusCode.OK, response_initial.StatusCode);
            Assert.IsNotNull(initialCustomerDetails);
            Assert.AreEqual("Some", initialCustomerDetails.FirstName);
        }

        [Test]
        public async Task GetProfileDetailsTest_Returns_NotFound()
        {
            // Arrange
            var testCustomerId = 99990000;

            // Act - For Initial Customer Profile data
            var response_initial = await TestClient.GetAsync($"{API_REQUESTS.GET_PROFILE_DETAILS}?customerId={testCustomerId}").ConfigureAwait(false);

            // Assert - For Initial Customer Profile data
            Assert.AreEqual(HttpStatusCode.NotFound, response_initial.StatusCode);
        }

        [Test]
        public async Task UpdateProfileDetailsTest_Add_And_Update_Returns_Ok()
        {
            // Arrange
            var testApplicationUserId = 1;
            var testCustomerId = 1;

            // Act - For Initial Customer Profile data
            var response_initial = await TestClient.GetAsync($"{API_REQUESTS.GET_PROFILE_DETAILS}?applicationUserId={testApplicationUserId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_initial.Content.ReadAsStringAsync().ConfigureAwait(false));
            var initialCustomerDetails = JsonConvert.DeserializeObject<CustomerDetailViewModel>(responseData.Data.ToString());

            // Assert - For Initial Customer Profile data
            Assert.AreEqual(HttpStatusCode.OK, response_initial.StatusCode);
            Assert.IsNotNull(initialCustomerDetails);
            Assert.AreEqual("Some", initialCustomerDetails.FirstName);

            // Arrange  - For updating Customer Profile data
            var data = new Dictionary<string, string>
            {
                { "CustomerId", testCustomerId.ToString() },
                { "FirstName", "test" },
                { "LastName", "user" },
                { "Email", "test@test1.com" },
                { "Phone", "12341618911" },
                { "DateOfBirth", "01-01-2000" }
            };

            // Act - For updating Customer Profile data
            var response_updated = await TestClient.PatchAsync(API_REQUESTS.UPDATE_PROFILE_DETAILS, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_updated.Content.ReadAsStringAsync().ConfigureAwait(false));
            var updatedCustomer = JsonConvert.DeserializeObject<CustomerDetailViewModel>(responseData.Data.ToString());

            // Assert - For updating Customer Profile data
            Assert.AreEqual(HttpStatusCode.OK, response_updated.StatusCode);
            Assert.IsNotNull(updatedCustomer);
            Assert.AreEqual("test@test1.com", updatedCustomer.Email);
        }

        [Test]
        public async Task UpdateProfileDetailsTest_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "WRONG_VALUE" },
                { "Email", "test@testcom" },
                { "Phone", "0610000000" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_PROFILE_DETAILS, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
