using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class FixLocationRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations");

            migrationBuilder.AddColumn<int>(
                name: "Bin_Id",
                table: "Stocks",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "Locations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId1",
                table: "Locations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Room_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Room_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Room_Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Room_Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Warehouse_Id = table.Column<int>(type: "integer", nullable: true),
                    LocationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Room_Id);
                    table.ForeignKey(
                        name: "FK_Room_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Room_Warehouses_Warehouse_Id",
                        column: x => x.Warehouse_Id,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Rack",
                columns: table => new
                {
                    Rack_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rack_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Rack_Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Rack_Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Room_Id = table.Column<int>(type: "integer", nullable: true),
                    LocationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rack", x => x.Rack_Id);
                    table.ForeignKey(
                        name: "FK_Rack_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Rack_Room_Room_Id",
                        column: x => x.Room_Id,
                        principalTable: "Room",
                        principalColumn: "Room_Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Shelf",
                columns: table => new
                {
                    Shelf_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Shelf_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Shelf_Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Shelf_Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Row_Id = table.Column<int>(type: "integer", nullable: true),
                    LocationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelf", x => x.Shelf_Id);
                    table.ForeignKey(
                        name: "FK_Shelf_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Shelf_Rack_Row_Id",
                        column: x => x.Row_Id,
                        principalTable: "Rack",
                        principalColumn: "Rack_Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Bin",
                columns: table => new
                {
                    Bin_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bin_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Bin_Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Bin_Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Shelf_Id = table.Column<int>(type: "integer", nullable: true),
                    LocationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bin", x => x.Bin_Id);
                    table.ForeignKey(
                        name: "FK_Bin_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bin_Shelf_Shelf_Id",
                        column: x => x.Shelf_Id,
                        principalTable: "Shelf",
                        principalColumn: "Shelf_Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Bin_Id",
                table: "Stocks",
                column: "Bin_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_WarehouseId1",
                table: "Locations",
                column: "WarehouseId1");

            migrationBuilder.CreateIndex(
                name: "IX_Bin_LocationId",
                table: "Bin",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Bin_Shelf_Id",
                table: "Bin",
                column: "Shelf_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rack_LocationId",
                table: "Rack",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rack_Room_Id",
                table: "Rack",
                column: "Room_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_LocationId",
                table: "Room",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_Warehouse_Id",
                table: "Room",
                column: "Warehouse_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shelf_LocationId",
                table: "Shelf",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Shelf_Row_Id",
                table: "Shelf",
                column: "Row_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId1",
                table: "Locations",
                column: "WarehouseId1",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Bin_Bin_Id",
                table: "Stocks",
                column: "Bin_Id",
                principalTable: "Bin",
                principalColumn: "Bin_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId1",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Bin_Bin_Id",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "Bin");

            migrationBuilder.DropTable(
                name: "Shelf");

            migrationBuilder.DropTable(
                name: "Rack");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_Bin_Id",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Locations_WarehouseId1",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Bin_Id",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "WarehouseId1",
                table: "Locations");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "Locations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
