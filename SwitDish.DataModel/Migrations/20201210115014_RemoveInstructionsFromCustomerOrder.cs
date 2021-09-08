using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class RemoveInstructionsFromCustomerOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "CustomerOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "CustomerOrders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
