using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitToStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Stocks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_UnitId",
                table: "Stocks",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Units_UnitId",
                table: "Stocks",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Units_UnitId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_UnitId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Stocks");
        }
    }
}
