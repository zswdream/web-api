using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Common.DomainModels;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture()]
    public class GetOrderDetailsControllerTests : API.Tests.IntegrationTestBase
    {
        [Test]
        public async Task GetOrderDetailsTest_OK()
        {
            // Arrange
            var testCustomerOrderId = 1;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_ORDER_DETAILS}?customerOrderId={testCustomerOrderId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var orderDetail = JsonConvert.DeserializeObject<CustomerOrderViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(orderDetail);
            Assert.AreEqual(110, orderDetail.GrandTotal);
        }

        [Test]
        public async Task GetOrderDetailsTest_NotFound()
        {
            // Arrange
            var testCustomerOrderId = 99999;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_ORDER_DETAILS}?customerOrderId={testCustomerOrderId}").ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}