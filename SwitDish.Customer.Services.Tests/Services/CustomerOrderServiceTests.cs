
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using NUnit.Framework;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Repositories;
using SwitDish.Common.Services;
using SwitDish.Customer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture()]
    public class CustomerOrderServiceTests : UnitTestBase
    {
        private DataModel.Models.Customer Test_Customer;
        private DataModel.Models.Vendor Test_Vendor;
        private DataModel.Models.CustomerDeliveryAddress Test_CustomerDeliveryaddress;
        private DataModel.Models.VendorOffer Test_VendorOffer ;
        private DataModel.Models.ProductCategory Test_ProductCategory;
        private DataModel.Models.Product Test_Product;

        private CustomerOrderService customerOrderService;
        
        [SetUp]
        public void SetUp()
        {
            this.dbContext = new DataModel.Models.SwitDishDbContext(this.dbOptions);
            this.dbContext.Database.OpenConnection();
            this.dbContext.Database.EnsureCreated();


            var newTestCustomer = new DataModel.Models.Customer
            {
                ApplicationUser = new DataModel.Models.ApplicationUser
                {
                    Email = "customer@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                    FirstName = "Customer",
                    LastName = "User",
                    Address = new DataModel.Models.Address
                    {
                        BuildingNumber = "123",
                        AddressLine1 = "abc street",
                        Country = "SOMECOUNTRY"
                    }
                }
            };
            this.dbContext.Customers.Add(newTestCustomer);
            this.Test_Customer = newTestCustomer;

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

            var newCustomerDeliveryaddress = new DataModel.Models.CustomerDeliveryAddress
            {
                CustomerId = this.Test_Customer.CustomerId,
                CompleteAddress = "xyz street",
                DeliveryArea = "xyz town",
                Instructions = "xyz instructions",
                CustomerDeliveryAddressType = DataModel.Enums.CustomerDeliveryAddressType.Home
            };
            this.dbContext.CustomerDeliveryAddresses.Add(newCustomerDeliveryaddress);
            this.Test_CustomerDeliveryaddress = newCustomerDeliveryaddress;

            var newVendorOffer = new DataModel.Models.VendorOffer
            {
                VendorId = this.Test_Vendor.VendorId,
                OfferCode = "123a",
                Description = "test description",
                Title = "free coupon",
                ValidFrom = DateTime.Now,
                ValidTill = DateTime.Now.AddDays(30),
                DiscountPercentage = 2,
                Image = "image.png",
                IsActive = true
            };
            this.dbContext.VendorOffers.Add(newVendorOffer);
            this.Test_VendorOffer = newVendorOffer;

            this.dbContext.SaveChanges();

            var newProductCategory = new DataModel.Models.ProductCategory
            {
                 Name = "Essential",
            };
            this.dbContext.ProductCategories.Add(newProductCategory);
            this.Test_ProductCategory = newProductCategory;

            this.dbContext.SaveChanges();
            
            var newProduct = new DataModel.Models.Product 
            {
                 Name = "Test Product",
                 Description = "Test desciption",
                 Price = 100,
                 Image = "product.png",
                 ProductCategoryId = this.Test_ProductCategory.ProductCategoryId,
                 VendorId = this.Test_Vendor.VendorId
            };
            this.dbContext.Products.Add(newProduct);
            this.Test_Product = newProduct;

            this.dbContext.SaveChanges();

            var customerOrderRepository = new CustomerOrderRepository(dbContext);
            var productRepository = new ProductRepository(dbContext);
            var vendorOfferRepository = new VendorOfferRepository(dbContext);
            var customerDeliveryAddressRepository = new CustomerDeliveryAddressRepository(dbContext);
            var customerRepository = new CustomerRepository(dbContext);
            var vendorRepository = new VendorRepository(dbContext);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var azureBlobClient = CloudStorageAccount.Parse(configuration.GetValue<string>("switdishcustomerimages-connectionstring")).CreateCloudBlobClient();
            var azureBlobService = new AzureBlobService(azureBlobClient);

            var customerService = new CustomerService(customerRepository, mapper,logger, azureBlobService, configuration);
            var vendorService = new VendorService(vendorRepository, mapper, logger);
            var productService = new ProductService(productRepository, mapper, logger);
            var vendorOfferService = new VendorOfferService(vendorOfferRepository, mapper, logger);
            var customerDeliveryAddressService = new CustomerDeliveryAddressService(customerDeliveryAddressRepository, mapper, logger, customerService);

            this.customerOrderService = new CustomerOrderService
            (
                customerOrderRepository,
                productService,
                vendorOfferService,
                customerDeliveryAddressService,
                customerService,
                vendorService,
                configuration,
                mapper,
                logger
            );
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetCustomerOrdersAsyncTest_Returns_1()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            var testVendor = this.Test_Vendor;
            var testCustomerDeliveryAddress = this.Test_CustomerDeliveryaddress;

            this.dbContext.CustomerOrders.Add(new DataModel.Models.CustomerOrder
            {
                CustomerId = testCustomer.CustomerId,
                VendorId = testVendor.VendorId,
                CustomerDeliveryAddressId = testCustomerDeliveryAddress.CustomerDeliveryAddressId,
                OrderedOn = DateTime.Now,
                DeliveredOn = DateTime.Today,
                RestaurantCharges = 10,
                DeliveryCharges = 100,
                OrderNumber = "12",
                Tax = 2,
                VendorOfferId = this.Test_VendorOffer.VendorOfferId,
                Status = DataModel.Enums.OrderStatus.RECEIVED,
                CustomerOrderProducts = new List<DataModel.Models.CustomerOrderProduct>
                {
                    new DataModel.Models.CustomerOrderProduct
                    {
                         ProductId = this.Test_Product.ProductId,
                         Quantity = 5
                    }
                }
            });
            this.dbContext.SaveChanges();

            // Act
            var result = await this.customerOrderService.GetCustomerOrdersAsync(testCustomer.CustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.Count());
        }
        [Test]
        public async Task GetCustomerOrdersAsyncTest_Returns_0()
        {
            // Arrange
            var testCustomerId = 8987;
           
            // Act
            var result = await this.customerOrderService.GetCustomerOrdersAsync(testCustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        [Test]
        public async Task GetCustomerOrdersAsyncTest_No_Record_Found()
        {
            // Arrange
            var testCustomerId = this.Test_Customer.CustomerId; 

            // Act
            var result = await this.customerOrderService.GetCustomerOrdersAsync(testCustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        [Test]
        public async Task GetCustomerOrderDetailsAsyncTest_Success()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            var testVendor = this.Test_Vendor;
            var testCustomerDeliveryAddress = this.Test_CustomerDeliveryaddress;

            var testCustomerOrder = new DataModel.Models.CustomerOrder
            {
                CustomerId = testCustomer.CustomerId,
                VendorId = testVendor.VendorId,
                CustomerDeliveryAddressId = testCustomerDeliveryAddress.CustomerDeliveryAddressId,
                OrderedOn = DateTime.Now,
                DeliveredOn = DateTime.Today,
                RestaurantCharges = 10,
                DeliveryCharges = 100,
                OrderNumber = "12",
                Tax = 2,
                VendorOfferId = this.Test_VendorOffer.VendorOfferId,
                Status = DataModel.Enums.OrderStatus.RECEIVED,
            };
            this.dbContext.CustomerOrders.Add(testCustomerOrder);
            this.dbContext.SaveChanges();

            // Act
            var result = await this.customerOrderService.GetCustomerOrderDetailsAsync(testCustomerOrder.CustomerOrderId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.OrderedOn, testCustomerOrder.OrderedOn);
        }
        [Test]
        public async Task GetCustomerOrderDetailsAsyncTest_Returns_Null()
        {
            // Arrange
            var testCustomerOrderId = 9999;

            // Act
            var result = await this.customerOrderService.GetCustomerOrderDetailsAsync(testCustomerOrderId).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
        [Test()]
        public async Task InsertCustomerOrderAsyncTest()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            var testVendor = this.Test_Vendor;
            var testCustomerDeliveryAddress = this.Test_CustomerDeliveryaddress;

            var testCustomerOrder = new CustomerOrder
            {
                CustomerId = testCustomer.CustomerId,
                VendorId = testVendor.VendorId,
                CustomerDeliveryAddressId = testCustomerDeliveryAddress.CustomerDeliveryAddressId,
                OrderedOn = DateTime.Now,
                RestaurantCharges = 10,
                DeliveryCharges = 100,
                Tax = 2,
                VendorOfferId = this.Test_VendorOffer.VendorOfferId,
                Status = DataModel.Enums.OrderStatus.RECEIVED,
                CustomerOrderProducts = new List<CustomerOrderProduct>
                {
                    new CustomerOrderProduct
                    {
                        ProductId = 1,
                        Quantity = 1000
                    }
                }
            };

            // Act
            var result = await this.customerOrderService.InsertCustomerOrderAsync(testCustomerOrder).ConfigureAwait(false);
            var resultOrder = result.Item1;
            // Assert
            Assert.IsNotNull(resultOrder);
            Assert.AreEqual(testCustomerOrder.CustomerId , resultOrder.CustomerId);
            Assert.AreEqual(testCustomerOrder.VendorId , resultOrder.VendorId);
            
        }
        [Test()]
        public async Task InsertCustomerOrderAsyncTest_CUSTOMER_NOT_FOUND()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            var testVendor = this.Test_Vendor;
            var testCustomerDeliveryAddress = this.Test_CustomerDeliveryaddress;

            var testInsertCustomerOrder = new CustomerOrder()
            {
                CustomerId = 298,
                VendorId = testVendor.VendorId,
                CustomerDeliveryAddressId = testCustomerDeliveryAddress.CustomerDeliveryAddressId,
                OrderedOn = DateTime.Now,
                DeliveredOn = DateTime.Today,
                RestaurantCharges = 10,
                DeliveryCharges = 100,
                OrderNumber = "12",
                Tax = 2,
                VendorOfferId = this.Test_VendorOffer.VendorOfferId,
                Status = DataModel.Enums.OrderStatus.RECEIVED,
            };
            this.dbContext.SaveChanges();


            // Act
            var result = await this.customerOrderService.InsertCustomerOrderAsync(testInsertCustomerOrder).ConfigureAwait(false);
            var resultOrder = result.Item2;
            // Assert
            Assert.IsNotNull(resultOrder);
            Assert.AreEqual("CUSTOMER_NOT_FOUND", resultOrder);
           
        }

        [Test()]
        public async Task InsertCustomerOrderAsyncTest_CUSTOMER_DELIVERY_NOT_FOUND()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            var testVendor = this.Test_Vendor;
            var testCustomerDeliveryAddress = this.Test_CustomerDeliveryaddress;

            var testInsertCustomerOrder = new CustomerOrder()
            {
                CustomerId = testCustomer.CustomerId,
                VendorId = testVendor.VendorId,
                CustomerDeliveryAddressId = 98,
                OrderedOn = DateTime.Now,
                DeliveredOn = DateTime.Today,
                RestaurantCharges = 10,
                DeliveryCharges = 100,
                OrderNumber = "12",
                Tax = 2,
                VendorOfferId = this.Test_VendorOffer.VendorOfferId,
                Status = DataModel.Enums.OrderStatus.RECEIVED,
                CustomerOrderProducts = new List<CustomerOrderProduct>
                {
                    new CustomerOrderProduct
                    {
                        ProductId = 1,
                        Quantity = 1000
                    }
                }
            };

            // Act
            var result = await this.customerOrderService.InsertCustomerOrderAsync(testInsertCustomerOrder).ConfigureAwait(false);
            var resultOrder = result.Item2;
            // Assert
            Assert.IsNotNull(resultOrder);
            Assert.AreEqual("CUSTOMER_DELIVERY_ADDRESS_NOT_FOUND", resultOrder);

        }

        [Test()]
        public async Task InsertCustomerOrderAsyncTest_VENDOR_NOT_FOUND()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            var testVendor = this.Test_Vendor;
            var testCustomerDeliveryAddress = this.Test_CustomerDeliveryaddress;

            var testInsertCustomerOrder = new CustomerOrder()
            {
                CustomerId = testCustomer.CustomerId,
                VendorId = 87,
                CustomerDeliveryAddressId = testCustomerDeliveryAddress.CustomerDeliveryAddressId,
                OrderedOn = DateTime.Now,
                DeliveredOn = DateTime.Today,
                RestaurantCharges = 10,
                DeliveryCharges = 100,
                OrderNumber = "12",
                Tax = 2,
                VendorOfferId = this.Test_VendorOffer.VendorOfferId,
                Status = DataModel.Enums.OrderStatus.RECEIVED,
            };
            this.dbContext.SaveChanges();


            // Act
            var result = await this.customerOrderService.InsertCustomerOrderAsync(testInsertCustomerOrder).ConfigureAwait(false);
            var resultOrder = result.Item2;
            // Assert
            Assert.IsNotNull(resultOrder);
            Assert.AreEqual("VENDOR_NOT_FOUND", resultOrder);

        }
        [Test]
        public async Task UpdateCustomerOrderAsyncTest()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            var testVendor = this.Test_Vendor;
            var testCustomerDeliveryAddress = this.Test_CustomerDeliveryaddress;

            var testCustomerOrder = new DataModel.Models.CustomerOrder()
            {
                CustomerId = testCustomer.CustomerId,
                VendorId = testVendor.VendorId,
                CustomerDeliveryAddressId = testCustomerDeliveryAddress.CustomerDeliveryAddressId,
                OrderedOn = DateTime.Now,
                DeliveredOn = DateTime.Today,
                RestaurantCharges = 10,
                DeliveryCharges = 100,
                OrderNumber = "12",
                Tax = 2,
                VendorOfferId = this.Test_VendorOffer.VendorOfferId,
                Status = DataModel.Enums.OrderStatus.RECEIVED,
            };
            this.dbContext.CustomerOrders.Add(testCustomerOrder);
            this.dbContext.SaveChanges();

            testCustomerOrder.VendorOfferId = null;

            // Act
            var result = await this.customerOrderService.UpdateCustomerOrderAsync(this.mapper.Map<Common.DomainModels.CustomerOrder>(testCustomerOrder)).ConfigureAwait(false);
            var resultOrder = result.Item1;

            // Assert
            Assert.IsNotNull(resultOrder);
            Assert.IsNull(resultOrder.VendorOfferId);
        }
        [Test]
        public async Task CancelCustomerOrderAsyncTest()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            var testVendor = this.Test_Vendor;
            var testCustomerDeliveryAddress = this.Test_CustomerDeliveryaddress;

            var testCancelCustomerOrder = new CustomerOrder()
            {
                CustomerId = testCustomer.CustomerId,
                VendorId = testVendor.VendorId,
                CustomerDeliveryAddressId = testCustomerDeliveryAddress.CustomerDeliveryAddressId,
                OrderedOn = DateTime.Now,
                DeliveredOn = DateTime.Today,
                RestaurantCharges = 10,
                DeliveryCharges = 100,
                OrderNumber = "12",
                Tax = 2,
                VendorOfferId = this.Test_VendorOffer.VendorOfferId,
                Status = DataModel.Enums.OrderStatus.CANCELLED,
            };
            this.dbContext.SaveChanges();


            // Act
            var result = await this.customerOrderService.CancelCustomerOrderAsync(testCancelCustomerOrder.CustomerOrderId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            

        }


    }
}