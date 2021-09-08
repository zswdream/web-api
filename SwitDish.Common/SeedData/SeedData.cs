using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web.LayoutRenderers;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace SwitDish.Common
{
    public static class SeedData
    {
        private readonly static List<ApplicationUser> Users = new List<ApplicationUser>{
            new ApplicationUser
            {
                ApplicationUserId = 1,
                Email = "someee.test@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                FirstName = "Some",
                LastName = "Body",
                Gender = "IDK",
                Phone = "12345678",
                Title = "X.",
                DateOfBirth = DateTime.Now,
                IsActive = true,
                Image = string.Empty
            },
            new ApplicationUser
            {
                ApplicationUserId = 2,
                Email = "switdish.test@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password321"),
                FirstName = "Some",
                LastName = "Consumer",
                Gender = "Male",
                Phone = "12345678",
                Title = "Mr.",
                DateOfBirth = DateTime.Now,
                IsActive = true,
                Image = string.Empty
            }
        };
        private readonly static List<Address> Addresses = new List<Address>
        {
            new Address
            {
                AddressId = 1,
                Active = true,
                AddressLine1 = "ABC Street",
                AddressLine2 = "XYZ City",
                BuildingNumber = "2",
                Country = "Sweden",
                City = "Unknown",
                State = "Unknown",
                FlatNumber = "123",
                PostCode = "321"
            },
            new Address
            {
                AddressId = 2,
                Active = true,
                AddressLine1 = "DEF Street",
                AddressLine2 = "QWE City",
                BuildingNumber = "2",
                Country = "Nigeria",
                City = "Unknown",
                State = "Unknown",
                FlatNumber = "321",
                PostCode = "4112",

            }
        };
        private readonly static List<Vendor> Vendors = new List<Vendor>
        {
            new Vendor {
                VendorId = 1,
                BrandName = "Alfatah",
                PrimaryEmail = "alfatah@gmail.com",
                DeliversFood = true,
                Name = "Alfatah Foods",
                VendorDeliveryTime = new VendorDeliveryTime {
                    Minimum = new TimeSpan(0,20,0),
                    Maximum = new TimeSpan(0,30,0)
                },
                VendorGalleryImages = new List<VendorGalleryImage>
                {
                    new VendorGalleryImage{Image = "abc.jpg"},
                    new VendorGalleryImage{Image = "def.jpg"},
                },
                VendorOffers = new List<VendorOffer>
                {
                    new VendorOffer 
                    {
                        VendorId = 1,
                        ValidFrom = DateTime.Now,
                        ValidTill = DateTime.Now.AddDays(30),
                        IsActive = true,
                        OfferCode = "ABC",
                        DiscountPercentage = 50m
                    },
                    new VendorOffer
                    {
                        VendorId = 1,
                        ValidFrom = DateTime.Now,
                        ValidTill = DateTime.Now.AddDays(-30),
                        IsActive = true,
                        OfferCode = "XYZ",
                        DiscountPercentage = 90m
                    }
                },
                VendorFeatures = new List<VendorFeature>
                {
                    new VendorFeature {
                        Feature = new Feature
                        {
                            Name = "Free Delivery"
                        }
                    }
                },
                VendorCuisines = new List<VendorCuisine>
                {
                    new VendorCuisine
                    {
                        Cuisine = new Cuisine { Name = "Chinese" }
                    }
                }
            },
            new Vendor
            {
                VendorId = 2,
                BrandName = "KFSea",
                VendorDeliveryTime = new VendorDeliveryTime {
                    Minimum = new TimeSpan(0,20,0),
                    Maximum = new TimeSpan(0,30,0)
                },
                PrimaryEmail = "kfsea@gmail.com",
                DeliversFood = true,
                Name = "KFSea Chicken"
            }
        };
        private readonly static List<Customer> Customers = new List<Customer>
        {
            new Customer
                        {
                            CustomerId = 1,
                            ApplicationUserId = 1
                        },
            new Customer
                        {
                            CustomerId = 2,
                            ApplicationUserId = 2
                        }
        };
        private readonly static List<SecurityQuestion> SecurityQuestions = new List<SecurityQuestion>
        {
            new SecurityQuestion {

                            SecurityQuestionId = 1,
                            Question = "What is your first pet name?"
                        },
            new SecurityQuestion
                        {
                            SecurityQuestionId = 2,
                            Question = "What is the first school you joined?"
                        },
            new SecurityQuestion
                        {
                            SecurityQuestionId = 3,
                            Question = "What is your nick name?"
                        }
        };
        private readonly static List<CustomerSecurityQuestion> CustomerSecurityQuestions = new List<CustomerSecurityQuestion>
        {
            new CustomerSecurityQuestion {
                             CustomerId = 1,
                             SecurityQuestionId = 1,
                             Answer = "Cat"
                        },
            new CustomerSecurityQuestion
                        {
                            CustomerId = 1,
                            SecurityQuestionId = 2,
                            Answer = "Dog"
                        },
            new CustomerSecurityQuestion
                        {
                            CustomerId = 3,
                            SecurityQuestionId = 3,
                            Answer = "IDK"
                        },
            new CustomerSecurityQuestion
                        {
                            CustomerId = 4,
                            SecurityQuestionId = 2,
                            Answer = "Peter"
                        },
            new CustomerSecurityQuestion
                        {
                            CustomerId = 4,
                            SecurityQuestionId = 3,
                            Answer = "David"
                        }
        };
        private readonly static List<CustomerFavouriteVendor> CustomerFavouriteVendors = new List<CustomerFavouriteVendor>
        {
            new CustomerFavouriteVendor
                        {
                            CustomerId = 1,
                            VendorId =1,
                        },
            new CustomerFavouriteVendor
                        {
                            CustomerId = 1,
                            VendorId = 2,
                        },
            new CustomerFavouriteVendor
                        {
                            CustomerId = 2,
                            VendorId = 2,
                        }
        };
        private readonly static List<ProductCategory> ProductCategories = new List<ProductCategory>
        {
            new ProductCategory {
                            ProductCategoryId = 1,
                            Name = "Fast Food"
                        },
            new ProductCategory
                        {
                            ProductCategoryId = 2,
                            Name = "Slow Food"
                        },
            new ProductCategory
                        {
                            ProductCategoryId = 3,
                            Name = "Chinese"
                        }
        };
        private readonly static List<Product> Products = new List<Product>
        {
            new Product {
                            ProductId = 1,
                            VendorId = 1,
                            ProductCategoryId = 1,
                            Name = "Fried Chicken",
                            Description = "Chicken that is deep fried in oil or whatever",
                            Price = 20
                        },
            new Product
                        {
                            ProductId = 2,
                            VendorId = 1,
                            ProductCategoryId = 1,
                            Name = "Chicken Nuggets",
                            Description = "Bite size nuggets that are also chicken, but boneless",
                            Price = 15
                        },
            new Product
                        {
                            ProductId = 3,
                            VendorId = 1,
                            ProductCategoryId = 2,
                            Name = "Arabian Rice",
                            Description = "Straight from the land of Arabs, probably.",
                            Price = 45
                        },
            new Product
                        {
                            ProductId = 4,
                            VendorId = 2,
                            ProductCategoryId = 3,
                            Name = "Chicken Chowmein",
                            Description = "Trying its best to appeat to be Chinese",
                            Price = 10
                        },
            new Product
                        {
                            ProductId = 5,
                            VendorId = 2,
                            ProductCategoryId = 3,
                            Name = "Egg Fried Rice",
                            Description = "Traditional, Boring, Authentic Rice with Eggs",
                            Price = 30
                        }
        };
        private readonly static List<CustomerOrder> CustomerOrders = new List<CustomerOrder>
        {
            new CustomerOrder {
                            CustomerOrderId = 1,
                            VendorId = 1,
                            CustomerId =1,
                            OrderedOn = DateTime.Now,
                            CustomerOrderProducts = new List<CustomerOrderProduct>
                            {
                                new CustomerOrderProduct {
                                    CustomerOrderId = 1,
                                    ProductId = 1,
                                    Quantity = 1
                                },
                                new CustomerOrderProduct {
                                    CustomerOrderId = 1,
                                    ProductId = 3,
                                    Quantity = 2
                                }
                            }
                        },
            new CustomerOrder
                        {
                            CustomerOrderId = 2,
                            VendorId = 1,
                            CustomerId = 1,
                            OrderedOn = DateTime.Now,
                            EditedOn = DateTime.MinValue,
                            CustomerOrderProducts = new List<CustomerOrderProduct>
                            {
                                new CustomerOrderProduct {
                                    CustomerOrderId = 2,
                                    ProductId = 2,
                                    Quantity = 3
                                },
                                new CustomerOrderProduct {
                                    CustomerOrderId = 2,
                                    ProductId = 4,
                                    Quantity = 1
                                }
                            }
                        },
            new CustomerOrder
                        {
                            CustomerOrderId = 3,
                            VendorId = 1,
                            CustomerId = 2,
                            OrderedOn = DateTime.Now,
                            CustomerOrderProducts = new List<CustomerOrderProduct>
                            {
                                new CustomerOrderProduct {
                                    CustomerOrderId = 3,
                                    ProductId = 4,
                                    Quantity = 2
                                },
                                new CustomerOrderProduct {
                                    CustomerOrderId = 3,
                                    ProductId = 2,
                                    Quantity = 2
                                }
                            }
                        },
            new CustomerOrder
                        {
                            CustomerOrderId = 4,
                            VendorId = 1,
                            CustomerId = 2,
                            Status = DataModel.Enums.OrderStatus.CANCELLED,
                            OrderedOn = DateTime.Now,
                            CustomerOrderProducts = new List<CustomerOrderProduct>
                            {
                                new CustomerOrderProduct {
                                    CustomerOrderId = 3,
                                    ProductId = 4,
                                    Quantity = 2
                                },
                                new CustomerOrderProduct {
                                    CustomerOrderId = 3,
                                    ProductId = 2,
                                    Quantity = 2
                                }
                            }
                        }
        };
        private readonly static List<CustomerDeliveryAddress> CustomerDeliveryAddresses = new List<CustomerDeliveryAddress>
        {
            new CustomerDeliveryAddress
                        {
                            CustomerDeliveryAddressId = 1,
                            CustomerId = 1,
                            CompleteAddress = "This place",
                            Instructions = "Im hungry",
                            DeliveryArea = "Other",
                            CustomerDeliveryAddressType = DataModel.Enums.CustomerDeliveryAddressType.Home
                        },
            new CustomerDeliveryAddress
                        {
                            CustomerDeliveryAddressId = 2,
                            CustomerId = 1,
                            CompleteAddress = "That place",
                            Instructions = "Please be quick",
                            DeliveryArea = "Another",
                            CustomerDeliveryAddressType = DataModel.Enums.CustomerDeliveryAddressType.Work
                        },
            new CustomerDeliveryAddress
                        {
                            CustomerDeliveryAddressId = 3,
                            CustomerId = 2,
                            CompleteAddress = "Another place",
                            Instructions = "Be here already...",
                            DeliveryArea = "The Void"
                        }
        };
        private readonly static List<CustomerOrderFeedback> CustomerOrderFeedbacks = new List<CustomerOrderFeedback>
        {
            new CustomerOrderFeedback
            {
                CustomerOrderFeedbackId = 1,
                CustomerId = 1,
                CustomerOrderId = 1,
                FoodRating = 4,
                FoodComments = "just wow"
            },
            new CustomerOrderFeedback
            {
                CustomerOrderFeedbackId = 2,
                CustomerId = 2,
                CustomerOrderId = 3,
                FoodRating = 3,
                FoodComments = "fine"
            }
        };
        private readonly static List<VendorFeedbackReaction> VendorFeedbackReactions = new List<VendorFeedbackReaction>
        {
            new VendorFeedbackReaction
            {
                CustomerId = 1,
                CustomerOrderFeedbackId = 1,
                ReactionType = DataModel.Enums.VendorFeedbackReactionType.Like
            },
            new VendorFeedbackReaction
            {
                CustomerId = 2,
                CustomerOrderFeedbackId = 2,
                ReactionType = DataModel.Enums.VendorFeedbackReactionType.Like
            }
        };

        public static void SeedTestData(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SwitDishDbContext>();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // Clear existing data
                context.CustomerOrderFeedbacks.RemoveRange(context.CustomerOrderFeedbacks);
                context.CustomerDeliveryAddresses.RemoveRange(context.CustomerDeliveryAddresses);
                context.CustomerOrderProducts.RemoveRange(context.CustomerOrderProducts);
                context.CustomerOrders.RemoveRange(context.CustomerOrders);
                context.Products.RemoveRange(context.Products);
                context.ProductCategories.RemoveRange(context.ProductCategories);
                context.CustomerFavouriteVendors.RemoveRange(context.CustomerFavouriteVendors);
                context.CustomerSecurityQuestions.RemoveRange(context.CustomerSecurityQuestions);
                context.SecurityQuestions.RemoveRange(context.SecurityQuestions);
                context.Customers.RemoveRange(context.Customers);
                context.Vendors.RemoveRange(context.Vendors);
                context.Addresses.RemoveRange(context.Addresses);
                context.ApplicationUsers.RemoveRange(context.ApplicationUsers);

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ApplicationUser] ON;");
                context.ApplicationUsers.AddRange(Users);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ApplicationUser] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Address] ON;");
                context.Addresses.AddRange(Addresses);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Address] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Vendor] ON;");
                context.Vendors.AddRange(Vendors);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Vendor] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Customer] ON;");
                context.Customers.AddRange(Customers);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Customer] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[SecurityQuestion] ON;");
                context.SecurityQuestions.AddRange(SecurityQuestions);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[SecurityQuestion] OFF;");

                context.CustomerSecurityQuestions.AddRange(CustomerSecurityQuestions);
                context.SaveChanges();

                context.CustomerFavouriteVendors.AddRange(CustomerFavouriteVendors);
                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductCategory] ON;");
                context.ProductCategories.AddRange(ProductCategories);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductCategory] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Product] ON;");
                context.Products.AddRange(Products);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Product] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[CustomerOrder] ON;");
                context.CustomerOrders.AddRange(CustomerOrders);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[CustomerOrder] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[CustomerDeliveryAddresses] ON;");
                context.CustomerDeliveryAddresses.AddRange(CustomerDeliveryAddresses);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[CustomerDeliveryAddresses] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[CustomerOrderFeedbacks] ON;");
                context.CustomerOrderFeedbacks.AddRange(CustomerOrderFeedbacks);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[CustomerOrderFeedbacks] OFF;");

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[VendorFeedbackReactions] ON;");
                context.VendorFeedbackReactions.AddRange(VendorFeedbackReactions);
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[VendorFeedbackReactions] OFF;");
                context.SaveChanges();

                transaction.Commit();
                context.SaveChanges();
            }
            catch
            {
                transaction.Rollback();
            }
        }
        public static void SeedTestData(SwitDishDbContext context)
        {
            // Dont seed data if any exists
            if (context.ApplicationUsers.Count() != 0)
                return;

            context.ApplicationUsers.AddRange(Users);
            context.Addresses.AddRange(Addresses);
            context.Vendors.AddRange(Vendors);
            context.Customers.AddRange(Customers);
            context.SecurityQuestions.AddRange(SecurityQuestions);
            context.CustomerSecurityQuestions.AddRange(CustomerSecurityQuestions);
            context.CustomerFavouriteVendors.AddRange(CustomerFavouriteVendors);
            context.ProductCategories.AddRange(ProductCategories);
            context.Products.AddRange(Products);
            context.CustomerOrders.AddRange(CustomerOrders);
            context.CustomerDeliveryAddresses.AddRange(CustomerDeliveryAddresses);
            context.CustomerOrderFeedbacks.AddRange(CustomerOrderFeedbacks);
            context.VendorFeedbackReactions.AddRange(VendorFeedbackReactions);
            context.SaveChanges();
        }
    }
}
