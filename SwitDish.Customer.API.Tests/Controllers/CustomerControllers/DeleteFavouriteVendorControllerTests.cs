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
    class DeleteFavouriteVendorControllerTests : API.Tests.IntegrationTestBase
    {
        [TestCase]
        public async Task DeleteCustomerFavouriteVendor_Returns_Ok()
        {
            // Arrange
            var query = "customerId=1&vendorId=1";

            // Act
            var response = await TestClient.DeleteAsync($"{API_REQUESTS.DELETE_FAVOURITE_VENDOR}?{query}").ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task DeleteCustomerFavouriteVendor_Invalid_Customer()
        {
            // Arrange
            var query = "customerId=12323&vendorId=2";

            // Act
            var response = await TestClient.DeleteAsync($"{API_REQUESTS.DELETE_FAVOURITE_VENDOR}?{query}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task DeleteCustomerFavouriteVendor_Invalid_Vendor()
        {
            // Arrange
            var query = "customerId=1&vendorId=23242";

            // Act
            var response = await TestClient.DeleteAsync($"{API_REQUESTS.DELETE_FAVOURITE_VENDOR}?{query}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
