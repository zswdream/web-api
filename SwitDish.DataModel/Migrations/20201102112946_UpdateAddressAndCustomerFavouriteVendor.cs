using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class UpdateAddressAndCustomerFavouriteVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerFavouriteVendors",
                table: "CustomerFavouriteVendors");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFavouriteVendors_CustomerId",
                table: "CustomerFavouriteVendors");

            migrationBuilder.DropColumn(
                name: "CustomerFavouriteVendorId",
                table: "CustomerFavouriteVendors");

            migrationBuilder.DropColumn(
                name: "County",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Addresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Addresses",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerFavouriteVendors",
                table: "CustomerFavouriteVendors",
                columns: new[] { "CustomerId", "VendorId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerFavouriteVendors",
                table: "CustomerFavouriteVendors");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "CustomerFavouriteVendorId",
                table: "CustomerFavouriteVendors",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerFavouriteVendors",
                table: "CustomerFavouriteVendors",
                column: "CustomerFavouriteVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFavouriteVendors_CustomerId",
                table: "CustomerFavouriteVendors",
                column: "CustomerId");
        }
    }
}
