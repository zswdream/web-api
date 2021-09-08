using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Repositories;
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
    public class VendorGalleryImageServiceTests : UnitTestBase
    {
        private DataModel.Models.Vendor Test_Vendor;
        private DataModel.Models.VendorGalleryImage Test_VendorGalleryImage_Rice;
        private DataModel.Models.VendorGalleryImage Test_VendorGalleryImage_Chicken;
        private VendorGalleryImageService vendorGalleryImageService;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new DataModel.Models.SwitDishDbContext(this.dbOptions);
            this.dbContext.Database.OpenConnection();
            this.dbContext.Database.EnsureCreated();


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
                Address = new DataModel.Models.Address
                {
                    AddressLine1 = "abc"
                }
            };
            this.dbContext.Vendors.Add(newTestVendor);
            this.Test_Vendor = newTestVendor;
            this.dbContext.SaveChanges();

            var vendorGalleryImage_Test_Rice = new DataModel.Models.VendorGalleryImage
            {
                VendorId = this.Test_Vendor.VendorId,
                Image = "rice.png",
            };
            this.dbContext.VendorGalleryImages.Add(vendorGalleryImage_Test_Rice);
            this.Test_VendorGalleryImage_Rice = vendorGalleryImage_Test_Rice;

            var vendorGalleryImage_Test_Chicken = new DataModel.Models.VendorGalleryImage
            {
                VendorId = this.Test_Vendor.VendorId,
                Image = "chicken.png",
            };
            this.dbContext.VendorGalleryImages.Add(vendorGalleryImage_Test_Chicken);
            this.Test_VendorGalleryImage_Chicken = vendorGalleryImage_Test_Chicken;

            this.dbContext.SaveChanges();
            
            var vendorGalleryImageRepository = new VendorGalleryImageRepository(dbContext);
            this.vendorGalleryImageService = new VendorGalleryImageService(vendorGalleryImageRepository, mapper, logger);
            this.DetachAllEntities();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task DeleteVendorGalleryAsyncTest()
        {
            // Arrange
            var testVendorGalleryImage = this.Test_VendorGalleryImage_Chicken;

            // Act
            var result = await this.vendorGalleryImageService.DeleteVendorGalleryAsync(this.mapper.Map<Common.DomainModels.VendorGalleryImage>(testVendorGalleryImage)).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task GetVendorGalleryAsyncTest_Success()
        {
            // Arrange
            var testVendorId = this.Test_Vendor.VendorId;

            // Act 
            var result = await this.vendorGalleryImageService.GetVendorGalleryAsync(testVendorId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2 , result.Count());
        }
        [Test]
        public async Task GetVendorGalleryAsyncTest_Returns_0()
        {
            // Arrange
            var testGetVendorGallery = 98954;

            // Act 
            var result = await this.vendorGalleryImageService.GetVendorGalleryAsync(testGetVendorGallery).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        [Test]
        public async Task InsertVendorGalleryAsyncTest_Success()
        {
            // Arrange
            var newVendorGallery = new Common.DomainModels.VendorGalleryImage() 
            { 
               Image = "image.png",
               VendorId = this.Test_Vendor.VendorId
            };

            // Act 
            var result = await this.vendorGalleryImageService.InsertVendorGalleryAsync(newVendorGallery).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newVendorGallery.Image, result.Image);
            Assert.AreEqual(newVendorGallery.VendorId, result.VendorId);
        }
        [Test]
        public async Task UpdateVendorGalleryAsyncTest_Success()
        {
            // Arrange
            var testUpdateVendorGallery = this.mapper.Map<Common.DomainModels.VendorGalleryImage>(this.Test_VendorGalleryImage_Rice);

            // Act
            testUpdateVendorGallery.Image = "new-rice.jpeg";
            var result = await this.vendorGalleryImageService.UpdateVendorGalleryAsync(testUpdateVendorGallery).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("new-rice.jpeg", result.Image);
            Assert.AreEqual(this.Test_Vendor.VendorId, result.VendorId);
        }
    }
}