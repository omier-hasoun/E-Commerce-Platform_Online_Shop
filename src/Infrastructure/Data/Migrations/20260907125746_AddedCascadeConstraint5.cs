using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeConstraint5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroups_Brands_BrandId",
                table: "ProductGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroups_Brands_BrandId",
                table: "ProductGroups",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroups_Brands_BrandId",
                table: "ProductGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroups_Brands_BrandId",
                table: "ProductGroups",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
