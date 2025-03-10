using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCategoryRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_categoryId",
                table: "Products",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_categories2_categoryId",
                table: "Products",
                column: "categoryId",
                principalTable: "categories2",
                onDelete: ReferentialAction.Cascade,
                principalColumn: "categoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_categories2_categoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_categoryId",
                table: "Products");
        }
    }
}
