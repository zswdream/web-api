using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlatNumber = table.Column<string>(nullable: true),
                    BuildingNumber = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "AppNLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Level = table.Column<string>(nullable: true),
                    Logger = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Userid = table.Column<int>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    Stacktrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cuisines",
                columns: table => new
                {
                    CuisineId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuisines", x => x.CuisineId);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    FeatureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.FeatureId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.ProductCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "SecurityQuestions",
                columns: table => new
                {
                    SecurityQuestionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityQuestions", x => x.SecurityQuestionId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCategories",
                columns: table => new
                {
                    ServiceCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCategories", x => x.ServiceCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "VendorDeliveryTimes",
                columns: table => new
                {
                    VendorDeliveryTimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum = table.Column<TimeSpan>(nullable: false),
                    Maximum = table.Column<TimeSpan>(nullable: false),
                    VendorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorDeliveryTimes", x => x.VendorDeliveryTimeId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    ApplicationUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.ApplicationUserId);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "ApplicationUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    BrandName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    PrimaryPhone = table.Column<string>(nullable: true),
                    SecondaryPhone = table.Column<string>(nullable: true),
                    PrimaryEmail = table.Column<string>(nullable: true),
                    SecondaryEmail = table.Column<string>(nullable: true),
                    MapLocation = table.Column<string>(nullable: true),
                    DeliversFood = table.Column<bool>(nullable: false),
                    FreeDelivery = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<int>(nullable: false),
                    VendorDeliveryTimeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                    table.ForeignKey(
                        name: "FK_Vendors_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "ApplicationUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vendors_VendorDeliveryTimes_VendorDeliveryTimeId",
                        column: x => x.VendorDeliveryTimeId,
                        principalTable: "VendorDeliveryTimes",
                        principalColumn: "VendorDeliveryTimeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDeliveryAddresses",
                columns: table => new
                {
                    CustomerDeliveryAddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    CompleteAddress = table.Column<string>(nullable: true),
                    DeliveryArea = table.Column<string>(nullable: true),
                    Instructions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDeliveryAddresses", x => x.CustomerDeliveryAddressId);
                    table.ForeignKey(
                        name: "FK_CustomerDeliveryAddresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSecurityQuestions",
                columns: table => new
                {
                    CustomerSecurityQuestionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    SecurityQuestionId = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSecurityQuestions", x => x.CustomerSecurityQuestionId);
                    table.ForeignKey(
                        name: "FK_CustomerSecurityQuestions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSecurityQuestions_SecurityQuestions_SecurityQuestionId",
                        column: x => x.SecurityQuestionId,
                        principalTable: "SecurityQuestions",
                        principalColumn: "SecurityQuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerBookings",
                columns: table => new
                {
                    CustomerBookingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBookings", x => x.CustomerBookingId);
                    table.ForeignKey(
                        name: "FK_CustomerBookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerBookings_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFavouriteVendors",
                columns: table => new
                {
                    CustomerFavouriteVendorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFavouriteVendors", x => x.CustomerFavouriteVendorId);
                    table.ForeignKey(
                        name: "FK_CustomerFavouriteVendors_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerFavouriteVendors_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    VendorId = table.Column<int>(nullable: false),
                    ProductCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "ProductCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorCuisines",
                columns: table => new
                {
                    VendorCuisineId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuisineId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorCuisines", x => x.VendorCuisineId);
                    table.ForeignKey(
                        name: "FK_VendorCuisines_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisines",
                        principalColumn: "CuisineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorCuisines_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorFeatures",
                columns: table => new
                {
                    VendorFeatureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorFeatures", x => x.VendorFeatureId);
                    table.ForeignKey(
                        name: "FK_VendorFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "FeatureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorFeatures_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorGalleryImages",
                columns: table => new
                {
                    VendorGalleryImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorGalleryImages", x => x.VendorGalleryImageId);
                    table.ForeignKey(
                        name: "FK_VendorGalleryImages_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorOffers",
                columns: table => new
                {
                    VendorOfferId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CouponCode = table.Column<string>(nullable: true),
                    DiscountPercentage = table.Column<decimal>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTill = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorOffers", x => x.VendorOfferId);
                    table.ForeignKey(
                        name: "FK_VendorOffers_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorServiceCategories",
                columns: table => new
                {
                    VendorServiceCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceCategoryId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorServiceCategories", x => x.VendorServiceCategoryId);
                    table.ForeignKey(
                        name: "FK_VendorServiceCategories_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "ServiceCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorServiceCategories_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrders",
                columns: table => new
                {
                    CustomerOrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    VendorOfferId = table.Column<int>(nullable: true),
                    OrderNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Instructions = table.Column<string>(nullable: true),
                    OrderedOn = table.Column<DateTime>(nullable: false),
                    DeliveredOn = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Tax = table.Column<decimal>(nullable: false),
                    DeliveryCharges = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrders", x => x.CustomerOrderId);
                    table.ForeignKey(
                        name: "FK_CustomerOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrders_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrders_VendorOffers_VendorOfferId",
                        column: x => x.VendorOfferId,
                        principalTable: "VendorOffers",
                        principalColumn: "VendorOfferId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderProducts",
                columns: table => new
                {
                    CustomerOrderProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerOrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderProducts", x => x.CustomerOrderProductId);
                    table.ForeignKey(
                        name: "FK_CustomerOrderProducts_CustomerOrders_CustomerOrderId",
                        column: x => x.CustomerOrderId,
                        principalTable: "CustomerOrders",
                        principalColumn: "CustomerOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_AddressId",
                table: "ApplicationUsers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBookings_CustomerId",
                table: "CustomerBookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBookings_VendorId",
                table: "CustomerBookings",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDeliveryAddresses_CustomerId",
                table: "CustomerDeliveryAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFavouriteVendors_CustomerId",
                table: "CustomerFavouriteVendors",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFavouriteVendors_VendorId",
                table: "CustomerFavouriteVendors",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderProducts_CustomerOrderId",
                table: "CustomerOrderProducts",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderProducts_ProductId",
                table: "CustomerOrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_CustomerId",
                table: "CustomerOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_VendorId",
                table: "CustomerOrders",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_VendorOfferId",
                table: "CustomerOrders",
                column: "VendorOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ApplicationUserId",
                table: "Customers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSecurityQuestions_CustomerId",
                table: "CustomerSecurityQuestions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSecurityQuestions_SecurityQuestionId",
                table: "CustomerSecurityQuestions",
                column: "SecurityQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId",
                table: "Products",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorCuisines_CuisineId",
                table: "VendorCuisines",
                column: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorCuisines_VendorId",
                table: "VendorCuisines",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorFeatures_FeatureId",
                table: "VendorFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorFeatures_VendorId",
                table: "VendorFeatures",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorGalleryImages_VendorId",
                table: "VendorGalleryImages",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorOffers_VendorId",
                table: "VendorOffers",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_ApplicationUserId",
                table: "Vendors",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_VendorDeliveryTimeId",
                table: "Vendors",
                column: "VendorDeliveryTimeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorServiceCategories_ServiceCategoryId",
                table: "VendorServiceCategories",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorServiceCategories_VendorId",
                table: "VendorServiceCategories",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppNLogs");

            migrationBuilder.DropTable(
                name: "CustomerBookings");

            migrationBuilder.DropTable(
                name: "CustomerDeliveryAddresses");

            migrationBuilder.DropTable(
                name: "CustomerFavouriteVendors");

            migrationBuilder.DropTable(
                name: "CustomerOrderProducts");

            migrationBuilder.DropTable(
                name: "CustomerSecurityQuestions");

            migrationBuilder.DropTable(
                name: "VendorCuisines");

            migrationBuilder.DropTable(
                name: "VendorFeatures");

            migrationBuilder.DropTable(
                name: "VendorGalleryImages");

            migrationBuilder.DropTable(
                name: "VendorServiceCategories");

            migrationBuilder.DropTable(
                name: "CustomerOrders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SecurityQuestions");

            migrationBuilder.DropTable(
                name: "Cuisines");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "ServiceCategories");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "VendorOffers");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "VendorDeliveryTimes");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
