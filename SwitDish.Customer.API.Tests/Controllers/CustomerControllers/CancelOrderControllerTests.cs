using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Tests;
using SwitDish.Customer.API.Utilities;
using SwitDish.DataModel.Enums;
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
    class CancelOrderControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task CancelOrder_OK()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerOrderId", "3" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.CANCEL_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task CancelOrder_Order_NotFound()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerOrderId", "9999" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.CANCEL_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
