using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SwitDish.DataModel.Models;

namespace SwitDish.Customer.API.Tests
{
    public class IntegrationTestBase
    {
        protected HttpClient TestClient {
            get
            {
                var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<DbContext>();
                        services.RemoveAll<SwitDishDbContext>();
                        services.RemoveAll<DbContextOptions>();
                        services.RemoveAll<DbContextOptionsBuilder>();

                        var dbServices = services.Where(d =>
                            d.ServiceType.Name.Contains("DbContext")
                            || d.ServiceType.Name.Contains("SwitDishDbContext")
                            || d.ServiceType.Name.Contains("DbContextOptions")
                            || d.ServiceType.Name.Contains("DbContextOptionsBuilder")
                            || d.ServiceType.Name.Contains("DbContextOptions")).ToList();

                        foreach (var service in dbServices)
                        {
                            services.Remove(service);
                        }

                        services.AddDbContext<SwitDishDbContext>(options =>
                            options.UseInMemoryDatabase(new Guid().ToString())
                            .UseLazyLoadingProxies());
                    });
                });

                // Seed Test Data
                using (var serviceScope = appFactory.Services.CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<SwitDishDbContext>();
                    Common.SeedData.SeedTestData(context);
                }

                return appFactory.CreateClient();
            }
        }
        
        protected void AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetJwtAsync());
        }

        private string GetJwtAsync()
        {
            return Common.Authorization.TokenGenerator.GenerateJSONWebToken(new Common.DomainModels.ApplicationUser
            {
                Email = "test@switdish.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345")
            },
            "SwitDish_Secret_Key_Used_For_JWT_Token_Encryption",
            "http://localhost:55456",
            "http://localhost:55456",
            60.0);
        }
    }
}