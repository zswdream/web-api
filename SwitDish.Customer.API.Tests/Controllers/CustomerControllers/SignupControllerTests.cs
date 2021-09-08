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
    class SignupControllerTests : API.Tests.IntegrationTestBase
    {
        [TestCase]
        public async Task SignupTest_Success()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" },
                { "EmailAddress", "test@new-email.com" },
                { "MobileNo", "12345678901" },
                { "DateOfBirth", "1990-01-02" },
                { "Password", "Password123" },
                { "ConfirmPassword", "Password123" },
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.CUSTOMER_SIGNUP, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var customer = JsonConvert.DeserializeObject<CustomerDetailViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreNotEqual(0, customer.CustomerId);
            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
            Assert.AreEqual("test@new-email.com", customer.Email);
        }

        [TestCase]
        public async Task SignupTest_Invalid_Details()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "LastName", "Doe" },
                { "EmailAddress", "12354" },
                { "MobileNo", "090078601" },
                { "Password", "password" },
                { "DateOfBirth", "1990-01-02" },
                { "ConfirmPassword", "password" }
            };
            var response = await TestClient.PostAsync(API_REQUESTS.CUSTOMER_SIGNUP, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestCase]
        public async Task SignupTest_EmailAddress_Already_Registered()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" },
                { "EmailAddress", "another@email.com" },
                { "DateOfBirth", "1990-01-02" },
                { "MobileNo", "090078601" },
                { "Password", "password" },
                { "ConfirmPassword", "password" }
            };
            var response = await TestClient.PostAsync(API_REQUESTS.CUSTOMER_SIGNUP, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestCase]
        public async Task SignupTest_PhoneNumber_Already_Registered()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" },
                { "EmailAddress", "another11@email.com" },
                { "DateOfBirth", "1990-01-02" },
                { "MobileNo", "090078601" },
                { "Password", "password" },
                { "ConfirmPassword", "password" }
            };
            var response = await TestClient.PostAsync(API_REQUESTS.CUSTOMER_SIGNUP, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
