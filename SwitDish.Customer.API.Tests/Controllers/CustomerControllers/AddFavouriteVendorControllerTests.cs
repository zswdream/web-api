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
    [TestFixture]
    class AddFavouriteVendorControllerTests : API.Tests.IntegrationTestBase
    {
        [Test]
        public async Task AddCustomerFavouriteVendor_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "customerId", "2" },
                { "vendorId", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_FAVOURITE_VENDOR, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task AddCustomerFavouriteVendor_Invalid_Customer()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "customerId", "122332" },
                { "vendorId", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_FAVOURITE_VENDOR, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task AddCustomerFavouriteVendor_Invalid_Vendor()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "customerId", "1" },
                { "vendorId", "12324345" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_FAVOURITE_VENDOR, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
