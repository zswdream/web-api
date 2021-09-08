using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class Fix_FK_In_Vendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_VendorDeliveryTimes_VendorDeliveryTimeId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_VendorDeliveryTimeId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorDeliveryTimeId",
                table: "Vendors");

            migrationBuilder.CreateIndex(
                name: "IX_VendorDeliveryTimes_VendorId",
                table: "VendorDeliveryTimes",
                column: "VendorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorDeliveryTimes_Vendors_VendorId",
                table: "VendorDeliveryTimes",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorDeliveryTimes_Vendors_VendorId",
                table: "VendorDeliveryTimes");

            migrationBuilder.DropIndex(
                name: "IX_VendorDeliveryTimes_VendorId",
                table: "VendorDeliveryTimes");

            migrationBuilder.AddColumn<int>(
                name: "VendorDeliveryTimeId",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_VendorDeliveryTimeId",
                table: "Vendors",
                column: "VendorDeliveryTimeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_VendorDeliveryTimes_VendorDeliveryTimeId",
                table: "Vendors",
                column: "VendorDeliveryTimeId",
                principalTable: "VendorDeliveryTimes",
                principalColumn: "VendorDeliveryTimeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
