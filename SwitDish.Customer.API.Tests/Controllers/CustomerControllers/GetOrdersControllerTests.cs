using Newtonsoft.Json;
using NUnit.Framework;
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
    class GetOrdersControllerTests : API.Tests.IntegrationTestBase
    {
        [TestCase(arg: 1, ExpectedResult = 3)]
        public async Task<int> GetOrdersTest(int customerId)
        {
            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_ORDERS}?customerId={customerId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var orders = JsonConvert.DeserializeObject<IEnumerable<CustomerOrderViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return orders.Count();
        }
    }
}
