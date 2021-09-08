using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class UpdateVendorAndFeedbackModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryCharges",
                table: "Vendors",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RestaurantCharges",
                table: "Vendors",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "VendorFeedbackReactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "CustomerOrderFeedbacks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCharges",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "RestaurantCharges",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "VendorFeedbackReactions");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "CustomerOrderFeedbacks");
        }
    }
}
