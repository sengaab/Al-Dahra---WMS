
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =====================================================
            // DEPARTMENT CODE
            // =====================================================

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Departments",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Give existing departments unique codes
            migrationBuilder.Sql("""
                UPDATE "Departments"
                SET "Code" = 'DEP-' || LPAD("DepartmentId"::text, 3, '0')
                WHERE "Code" = '';
                """);

            // Unique Department Code
            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true);

            // =====================================================
            // STOCK -> LOCATION
            // =====================================================

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Locations_LocationId",
                table: "Stocks");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Locations_LocationId",
                table: "Stocks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // =====================================================
            // STOCK -> LOCATION
            // =====================================================

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Locations_LocationId",
                table: "Stocks");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Locations_LocationId",
                table: "Stocks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            // =====================================================
            // DEPARTMENT CODE
            // =====================================================

            migrationBuilder.DropIndex(
                name: "IX_Departments_Code",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Departments");
        }
    }
}

