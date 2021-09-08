using Microsoft.EntityFrameworkCore.Migrations;

namespace SwitDish.DataModel.Migrations
{
    public partial class RemoveVendorFeedbackAndAddReactionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorFeedbacks");

            migrationBuilder.CreateTable(
                name: "VendorFeedbackReactions",
                columns: table => new
                {
                    VendorFeedbackReactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerOrderFeedbackId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ReactionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorFeedbackReactions", x => x.VendorFeedbackReactionId);
                    table.ForeignKey(
                        name: "FK_VendorFeedbackReactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VendorFeedbackReactions_CustomerOrderFeedbacks_CustomerOrderFeedbackId",
                        column: x => x.CustomerOrderFeedbackId,
                        principalTable: "CustomerOrderFeedbacks",
                        principalColumn: "CustomerOrderFeedbackId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorFeedbackReactions_CustomerId",
                table: "VendorFeedbackReactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorFeedbackReactions_CustomerOrderFeedbackId",
                table: "VendorFeedbackReactions",
                column: "CustomerOrderFeedbackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorFeedbackReactions");

            migrationBuilder.CreateTable(
                name: "VendorFeedbacks",
                columns: table => new
                {
                    VendorFeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorFeedbacks_CustomerId",
                table: "VendorFeedbacks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorFeedbacks_VendorId",
                table: "VendorFeedbacks",
                column: "VendorId");
        }
    }
}
