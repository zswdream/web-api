using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Repositories;
using SwitDish.Common.ViewModels;
using SwitDish.Customer.Services;
using SwitDish.Customer.Services.Tests;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture()]
    public class VendorServiceTests : UnitTestBase
    {
        private DataModel.Models.Vendor Test_Vendor;
        private VendorService vendorService;

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
            this.Test_Vendor = newTestVendor;

            var vendorRepository = new VendorRepository(dbContext);
            this.vendorService = new VendorService(vendorRepository, mapper, logger);
            this.DetachAllEntities();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetAllVendorsAsyncTests()
        {
            // Act
            var result = await this.vendorService.GetAllVendorsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
        [Test]
        public async Task GetVendorByIdAsyncTests_Success()
        {
            // Arrange
            var testVendor = this.Test_Vendor;

            // Act
            var result = await this.vendorService.GetVendorByIdAsync(testVendor.VendorId).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(testVendor.Name, result.Name);
            Assert.AreEqual(testVendor.PrimaryEmail, result.PrimaryEmail);
            Assert.AreEqual(testVendor.BrandName, result.BrandName);
        }
        [Test]
        public async Task GetVendorByIdAsyncTests_Returns_Null()
        {
            // Arrange
            var testVendorId = 989795;

            // Act
            var result = await this.vendorService.GetVendorByIdAsync(testVendorId).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetAllVendorsAsync_Filtered_Tests_Returns_1()
        {
            // Arrange
            var testFilters = new VendorFiltersViewModel
            {
                Cuisines = new List<string> { "Fast Food" },
                Features = new List<string> { "Free Delivery" }
            };

            // Act
            var result = await this.vendorService.GetAllVendorsAsync(testFilters).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
        [Test]
        public async Task GetAllVendorsAsync_Filtered_Tests_Returns_0()
        {
            // Arrange
            var testFilters = new VendorFiltersViewModel
            {
                Cuisines = new List<string> { "Some Strange Cuisine" },
                Features = new List<string> { "Unavailable Feature that is not available from any Vendor" }
            };

            // Act
            var result = await this.vendorService.GetAllVendorsAsync(testFilters).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        [Test]
        public async Task GetVendorListFiltersAsync_Success()
        {
            // Act
            var result = await this.vendorService.GetVendorListFiltersAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}