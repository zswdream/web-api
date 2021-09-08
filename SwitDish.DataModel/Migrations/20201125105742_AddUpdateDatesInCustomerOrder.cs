using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class AddUpdateDatesInCustomerOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveredOn",
                table: "CustomerOrders",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedOn",
                table: "CustomerOrders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "CustomerOrders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveredOn",
                table: "CustomerOrders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
