using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class AddFeedbackModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "VendorOffers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerOrders");

            migrationBuilder.AddColumn<string>(
                name: "OfferCode",
                table: "VendorOffers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerDeliveryAddressId",
                table: "CustomerOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RestaurantCharges",
                table: "CustomerOrders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CustomerOrderFeedbacks",
                columns: table => new
                {
                    CustomerOrderFeedbackId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerOrderId = table.Column<int>(nullable: false),
                    FoodRating = table.Column<int>(nullable: false),
                    AppRating = table.Column<int>(nullable: false),
                    DeliveryAgentRating = table.Column<int>(nullable: false),
                    FoodComments = table.Column<string>(nullable: true),
                    AppComments = table.Column<string>(nullable: true),
                    DeliveryAgentComments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderFeedbacks", x => x.CustomerOrderFeedbackId);
                    table.ForeignKey(
                        name: "FK_CustomerOrderFeedbacks_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderFeedbacks_CustomerOrders_CustomerOrderId",
                        column: x => x.CustomerOrderId,
                        principalTable: "CustomerOrders",
                        principalColumn: "CustomerOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorFeedbacks",
                columns: table => new
                {
                    VendorFeedbackId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorFeedbacks", x => x.VendorFeedbackId);
                    table.ForeignKey(
                        name: "FK_VendorFeedbacks_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorFeedbacks_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrders_CustomerDeliveryAddressId",
                table: "CustomerOrders",
                column: "CustomerDeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderFeedbacks_CustomerId",
                table: "CustomerOrderFeedbacks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderFeedbacks_CustomerOrderId",
                table: "CustomerOrderFeedbacks",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorFeedbacks_CustomerId",
                table: "VendorFeedbacks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorFeedbacks_VendorId",
                table: "VendorFeedbacks",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrders_CustomerDeliveryAddresses_CustomerDeliveryAddressId",
                table: "CustomerOrders",
                column: "CustomerDeliveryAddressId",
                principalTable: "CustomerDeliveryAddresses",
                principalColumn: "CustomerDeliveryAddressId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerOrders_CustomerDeliveryAddresses_CustomerDeliveryAddressId",
                table: "CustomerOrders");

            migrationBuilder.DropTable(
                name: "CustomerOrderFeedbacks");

            migrationBuilder.DropTable(
                name: "VendorFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_CustomerOrders_CustomerDeliveryAddressId",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "OfferCode",
                table: "VendorOffers");

            migrationBuilder.DropColumn(
                name: "CustomerDeliveryAddressId",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "RestaurantCharges",
                table: "CustomerOrders");

            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "VendorOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerOrders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
