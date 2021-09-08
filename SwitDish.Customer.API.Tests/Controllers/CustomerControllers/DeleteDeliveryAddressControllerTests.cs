using NUnit.Framework;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture()]
    class DeleteDeliveryAddressControllerTests : API.Tests.IntegrationTestBase
    {
        [TestCase(arg: 2, ExpectedResult = HttpStatusCode.OK)]
        [TestCase(arg: 999, ExpectedResult = HttpStatusCode.NotFound)]
        public async Task<HttpStatusCode> DeleteDeliveryAddressTest(long customerDeliveryAddressId)
        {
            var response = await TestClient.DeleteAsync($"{API_REQUESTS.DELETE_DELIVERY_ADDRESS}?customerDeliveryAddressId={customerDeliveryAddressId}").ConfigureAwait(false);
            return response.StatusCode;
        }
    }
}
