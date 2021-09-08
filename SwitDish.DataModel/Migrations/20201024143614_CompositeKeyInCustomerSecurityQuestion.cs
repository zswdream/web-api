using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class CompositeKeyInCustomerSecurityQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_ApplicationUsers_ApplicationUserId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_ApplicationUserId",
                table: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerSecurityQuestions",
                table: "CustomerSecurityQuestions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSecurityQuestions_CustomerId",
                table: "CustomerSecurityQuestions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CustomerSecurityQuestionId",
                table: "CustomerSecurityQuestions");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Vendors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AvatarImage",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerSecurityQuestions",
                table: "CustomerSecurityQuestions",
                columns: new[] { "CustomerId", "SecurityQuestionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_AddressId",
                table: "Vendors",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Addresses_AddressId",
                table: "Vendors",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Addresses_AddressId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_AddressId",
                table: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerSecurityQuestions",
                table: "CustomerSecurityQuestions");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "AvatarImage",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Vendors");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerSecurityQuestionId",
                table: "CustomerSecurityQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerSecurityQuestions",
                table: "CustomerSecurityQuestions",
                column: "CustomerSecurityQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_ApplicationUserId",
                table: "Vendors",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSecurityQuestions_CustomerId",
                table: "CustomerSecurityQuestions",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_ApplicationUsers_ApplicationUserId",
                table: "Vendors",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
