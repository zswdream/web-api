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
    [TestFixture]
    class UpdateDeliveryAddressControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task UpdateCustomerDeliveryAddressTest_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerDeliveryAddressId", "1" },
                { "CustomerId", "1" },
                { "Instructions", "Please deliver quick" },
                { "CompleteAddress", "My House, This Street, That City" },
                { "DeliveryArea", "SomeArea" },
                { "Type", "Home" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_DELIVERY_ADDRESS, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var customerDeliveryAddress = JsonConvert.DeserializeObject<CustomerDeliveryAddressViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Please deliver quick", customerDeliveryAddress.Instructions);
            Assert.IsNotNull(customerDeliveryAddress);
        }

        [Test]
        public async Task UpdateCustomerDeliveryAddressTest_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerDeliveryAddressId", "abc" },
                { "Instructions", "Please deliver quick" },
                { "CompleteAddress", "My House, This Street, That City" },
                { "DeliveryArea", "SomeArea" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_DELIVERY_ADDRESS, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task UpdateCustomerDeliveryAddressTest_Returns_NotFound()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerDeliveryAddressId", "3" },
                { "CustomerId", "1" },
                { "Instructions", "Please deliver quick" },
                { "CompleteAddress", "My House, This Street, That City" },
                { "DeliveryArea", "SomeArea" },
                { "Type", "Home" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_DELIVERY_ADDRESS, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
