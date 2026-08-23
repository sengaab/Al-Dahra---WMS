using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace whm.Migrations
{
    /// <inheritdoc />
    public partial class AddStockExpiryDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =========================================================
            // UnitId -> Nullable
            // =========================================================

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Stocks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");


            // =========================================================
            // Bin_Id -> Nullable
            // =========================================================

            migrationBuilder.AlterColumn<int>(
                name: "Bin_Id",
                table: "Stocks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");


            // =========================================================
            // Expiry Date
            // =========================================================

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Stocks",
                type: "timestamp with time zone",
                nullable: true);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // =========================================================
            // Remove ExpiryDate
            // =========================================================

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Stocks");


            // =========================================================
            // UnitId -> Required
            // =========================================================

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Stocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);


            // =========================================================
            // Bin_Id -> Required
            // =========================================================

            migrationBuilder.AlterColumn<int>(
                name: "Bin_Id",
                table: "Stocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}