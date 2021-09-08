using NUnit.Framework;
using SwitDish.DataModel_OLD.Models;
using SwitDish.Vendor.API.Tests.DataArranger;
using SwitDish.Vendor.API.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SwitDish.Vendor.API.Tests.Controllers
{
    [TestFixture]
    [Explicit("Ignore test on the build pipeline")]
    public class ApiAgentControllerTests : Utilities.ApiIntegrationTestsBase
    {
        [Test]
        public void ApiAgentController_Get_ById_Ok()
        {
            var dbAddress = AddressArranger.DefaultTestData();
            using (var addressArranger = new AddressArranger(this.SetupDbContext, dbAddress, DataArrangerBase.Behaviour.CreateEntity))
            using (var vendorArranger = new VendorArranger(this.SetupDbContext, VendorArranger.CreateTestData("Test Vendor", addressArranger), DataArrangerBase.Behaviour.CreateEntity))
            {
                string firstName = "Saheed";
                string lastName = "Busari";
                string userName = $"{Guid.NewGuid()}@gmail.com";
                string email = userName;
                var dbUser = UserArranger.CreateTestData(firstName, lastName, userName, email);

                using (var userArranger = new UserArranger(this.SetupDbContext, dbUser, DataArrangerBase.Behaviour.CreateEntity))
                {
                    var dbCompany = CompanyArranger.DefaultTestData();
                    using (var companyArranger = new CompanyArranger(this.SetupDbContext, dbCompany, DataArrangerBase.Behaviour.CreateEntity))
                    {
                        var dbAgent = AgentArranger.CreateTestData(companyArranger.Entity.CompanyId,
                           //(int)vendorArranger.Entity.VendorId,
                           Convert.ToInt32(vendorArranger.Entity.VendorId),

                            userArranger.Entity.UserId);

                        using (var agentArranger = new AgentArranger(this.SetupDbContext, dbAgent, DataArrangerBase.Behaviour.CreateEntity))
                        {
                            string expectedJson = GetTestJsonFile()
                                .Replace(@"""<agentId>""", agentArranger.Entity.AgentId.ToString())
                                .Replace(@"<agentFirstName>", userArranger.Entity.FirstName)
                                .Replace(@"<agentLastName>", userArranger.Entity.LastName)
                                .Replace(@"<userName>", userArranger.Entity.Username)
                                .Replace(@"<email>", userArranger.Entity.Email)
                                .Replace(@"<password>", userArranger.Entity.Password)
                                .Replace(@"<phone>", userArranger.Entity.Phone)
                                .Replace(@"<gender>", userArranger.Entity.Gender)
                                .Replace(@"<title>", userArranger.Entity.Title)
                                .Replace(@"""<userId>""", userArranger.Entity.UserId.ToString())
                                .Replace(@"""<companyId>""", companyArranger.Entity.CompanyId.ToString())
                                .Replace(@"""<vendorId>""", vendorArranger.Entity.Name)
                                .Replace(@"""<isActive>""", userArranger.Entity.Active.ToString().ToLower())
                                .Replace(@"<dateRegistered>", userArranger.Entity.Active.ToString().ToLower())
                                ;

                            var httpClient = new HttpClient(this.Server);
                            string route = $"/api/agents/get?agentid={agentArranger.Entity.AgentId}";

                            HttpRequestMessage getRequest = this.CreateRequest(route, HttpMethod.Get);
                            using (HttpResponseMessage response = httpClient.SendAsync(getRequest).Result)
                            {
                                Assert.IsNotNull(response.Content);
                                Assert.IsTrue(response.IsSuccessStatusCode);
                                Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
                                string result = BlankDateField(response.Content.ReadAsStringAsync().Result, "DateRegistered");
                                string prettyPrintedResult = Json.PrettyPrint(result);
                                Assert.AreEqual(expectedJson, prettyPrintedResult);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [Explicit("Ignore test on the build pipeline")]
        public void ApiAgentController_Post_Ok()
        {
            long deletedAgentId = 0;

            var dbAddress = AddressArranger.DefaultTestData();
            using (var addressArranger = new AddressArranger(this.SetupDbContext, dbAddress, DataArrangerBase.Behaviour.CreateEntity))
            using (var vendorArranger = new VendorArranger(this.SetupDbContext, VendorArranger.CreateTestData("Test Vendor", addressArranger), DataArrangerBase.Behaviour.CreateEntity))
            {
                string firstName = "Saheed";
                string lastName = "Busari";
                string userName = $"{Guid.NewGuid()}@gmail.com";
                string email = userName;
                var dbUser = UserArranger.CreateTestData(firstName, lastName, userName, email);

                using (var userArranger = new UserArranger(this.SetupDbContext, dbUser, DataArrangerBase.Behaviour.CreateEntity))
                {
                    var dbCompany = CompanyArranger.DefaultTestData();
                    using (var companyArranger = new CompanyArranger(this.SetupDbContext, dbCompany, DataArrangerBase.Behaviour.CreateEntity))
                    {

                        string agentJson = GetTestJsonFile()
                            .Replace(@"""<agentId>""", "0")
                            .Replace(@"<agentFirstName>", userArranger.Entity.FirstName)
                            .Replace(@"<agentLastName>", userArranger.Entity.LastName)
                            .Replace(@"<userName>", userArranger.Entity.Username)
                            .Replace(@"<email>", userArranger.Entity.Email)
                            .Replace(@"<password>", userArranger.Entity.Password)
                            .Replace(@"<phone>", userArranger.Entity.Phone)
                            .Replace(@"<gender>", userArranger.Entity.Gender)
                            .Replace(@"<title>", userArranger.Entity.Title)
                            .Replace(@"""<userId>""", userArranger.Entity.UserId.ToString())
                            .Replace(@"""<companyId>""", companyArranger.Entity.CompanyId.ToString())
                            .Replace(@"""<vendorId>""", vendorArranger.Entity.VendorId.ToString())
                            .Replace(@"<vendorName>", vendorArranger.Entity.Name)
                            .Replace(@"""<isActive>""", userArranger.Entity.Active.ToString().ToLower())
                            .Replace(@"<dateRegistered>", userArranger.Entity.Active.ToString().ToLower())
                            ;

                        var httpClient = new HttpClient(this.Server);
                        string route = $"/api/agents/put";

                        HttpRequestMessage postRequest = this.CreateRequest(route, HttpMethod.Put);
                        postRequest.Content = new StringContent(agentJson, Encoding.UTF8, "application/json");
                        using (HttpResponseMessage response = httpClient.SendAsync(postRequest).Result)
                        {
                            Assert.IsNotNull(response.Content);
                            Assert.IsTrue(response.IsSuccessStatusCode);
                            string result = BlankDateField(response.Content.ReadAsStringAsync().Result, "DateRegistered");
                            string prettyPrintedResult = Json.PrettyPrint(result);
                            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);

                            var agent = Json.Deserialise<Agent>(prettyPrintedResult);
                            deletedAgentId = agent.AgentId;
                            // delete agent so that other entities can be deleted
                            route = $"/api/agents/delete?agentid={agent.AgentId}";
                            HttpRequestMessage deleteRequest = this.CreateRequest(route, HttpMethod.Delete);
                            HttpResponseMessage deleteResponse = httpClient.SendAsync(deleteRequest).Result;
                        }

                        // delete the 
                    }
                }
            }
        }
    }
}
