using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Tests;
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
    class GetDeliveryAddressesControllerTests : IntegrationTestBase
    {
        [TestCase(arg: 1, ExpectedResult = 2)]
        public async Task<int> GetDeliveryAddressesTest(int customerId)
        {
            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_DELIVERY_ADDRESSES}?customerId={customerId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var orders = JsonConvert.DeserializeObject<IEnumerable<CustomerDeliveryAddressViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return orders.Count();
        }
    }
}
