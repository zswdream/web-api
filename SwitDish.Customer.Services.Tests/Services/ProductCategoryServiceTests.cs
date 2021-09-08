using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwitDish.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture]
    public class ProductCategoryServiceTests : UnitTestBase
    {
        private ProductCategoryService productCategoryService;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new DataModel.Models.SwitDishDbContext(this.dbOptions);
            this.dbContext.Database.OpenConnection();
            this.dbContext.Database.EnsureCreated();

            var pd_one = new DataModel.Models.ProductCategory { Name = "Fast Food" };
            var pd_two = new DataModel.Models.ProductCategory { Name = "Continental" };
            this.dbContext.ProductCategories.Add(pd_one);
            this.dbContext.ProductCategories.Add(pd_two);
            this.dbContext.SaveChanges();

            var newTestVendor = new DataModel.Models.Vendor
            {
                Name = "ABC Vendor",
                BrandName = "ABC Brand",
                PrimaryEmail = "Email.gmail.com",
                VendorDeliveryTime = new DataModel.Models.VendorDeliveryTime
                {
                    Minimum = new TimeSpan(0, 20, 0),
                    Maximum = new TimeSpan(0, 30, 0)
                },
                VendorCuisines = new List<DataModel.Models.VendorCuisine>
                {
                    new DataModel.Models.VendorCuisine
                    {
                        Cuisine = new DataModel.Models.Cuisine{Name="Fast Food"}
                    }
                },
                VendorFeatures = new List<DataModel.Models.VendorFeature> {
                    new DataModel.Models.VendorFeature
                    {
                        Feature = new DataModel.Models.Feature{Name="Free Delivery"}
                    }
                },
                Address = new DataModel.Models.Address
                {
                    AddressLine1 = "abc"
                },
                Products = new List<DataModel.Models.Product>
                {
                    new DataModel.Models.Product
                    {
                        ProductCategoryId = pd_one.ProductCategoryId
                    }
                }
            };
            this.dbContext.Vendors.Add(newTestVendor);
            this.dbContext.SaveChanges();

            this.dbContext.Products.Add(new DataModel.Models.Product { ProductCategoryId = pd_one.ProductCategoryId, VendorId = newTestVendor.VendorId });
            this.dbContext.Products.Add(new DataModel.Models.Product { ProductCategoryId = pd_two.ProductCategoryId, VendorId = newTestVendor.VendorId });
            this.dbContext.SaveChanges();

            var productRepository = new ProductRepository(dbContext);
            var productService = new ProductService(productRepository, mapper, logger);
            this.productCategoryService = new ProductCategoryService(productService, mapper, logger);
            this.DetachAllEntities();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetProductCategories_Success()
        {
            // Act
            var result = await this.productCategoryService.GetProductCategories().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}