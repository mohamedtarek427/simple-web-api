using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class correctProductColName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_categories2_categoryId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_categories2_categoryId",
                table: "Products",
                column: "categoryId",
                principalTable: "categories2",
                principalColumn: "categoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_categories2_categoryId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_categories2_categoryId",
                table: "Products",
                column: "categoryId",
                principalTable: "categories2",
                principalColumn: "categoryId");
        }
    }
}
