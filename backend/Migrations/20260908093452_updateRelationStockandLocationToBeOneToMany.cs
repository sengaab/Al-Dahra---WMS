using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationStockandLocationToBeOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stocks_LocationId",
                table: "Stocks");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_LocationId",
                table: "Stocks",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stocks_LocationId",
                table: "Stocks");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_LocationId",
                table: "Stocks",
                column: "LocationId",
                unique: true);
        }
    }
}
