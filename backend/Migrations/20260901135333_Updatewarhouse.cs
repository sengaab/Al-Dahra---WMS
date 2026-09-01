using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class Updatewarhouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bin_Locations_LocationId",
                table: "Bin");

            migrationBuilder.DropForeignKey(
                name: "FK_Bin_Shelf_Shelf_Id",
                table: "Bin");

            migrationBuilder.DropForeignKey(
                name: "FK_Rack_Locations_LocationId",
                table: "Rack");

            migrationBuilder.DropForeignKey(
                name: "FK_Rack_Room_Room_Id",
                table: "Rack");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Locations_LocationId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Warehouses_Warehouse_Id",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelf_Locations_LocationId",
                table: "Shelf");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelf_Rack_Row_Id",
                table: "Shelf");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Bin_Bin_Id",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shelf",
                table: "Shelf");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rack",
                table: "Rack");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bin",
                table: "Bin");

            migrationBuilder.RenameTable(
                name: "Shelf",
                newName: "Shelves");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "Rack",
                newName: "Racks");

            migrationBuilder.RenameTable(
                name: "Bin",
                newName: "Bins");

            migrationBuilder.RenameIndex(
                name: "IX_Shelf_Row_Id",
                table: "Shelves",
                newName: "IX_Shelves_Row_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Shelf_LocationId",
                table: "Shelves",
                newName: "IX_Shelves_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_Warehouse_Id",
                table: "Rooms",
                newName: "IX_Rooms_Warehouse_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Room_LocationId",
                table: "Rooms",
                newName: "IX_Rooms_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Rack_Room_Id",
                table: "Racks",
                newName: "IX_Racks_Room_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Rack_LocationId",
                table: "Racks",
                newName: "IX_Racks_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Bin_Shelf_Id",
                table: "Bins",
                newName: "IX_Bins_Shelf_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Bin_LocationId",
                table: "Bins",
                newName: "IX_Bins_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shelves",
                table: "Shelves",
                column: "Shelf_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Room_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Racks",
                table: "Racks",
                column: "Rack_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bins",
                table: "Bins",
                column: "Bin_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_Locations_LocationId",
                table: "Bins",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_Shelves_Shelf_Id",
                table: "Bins",
                column: "Shelf_Id",
                principalTable: "Shelves",
                principalColumn: "Shelf_Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Locations_LocationId",
                table: "Racks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Rooms_Room_Id",
                table: "Racks",
                column: "Room_Id",
                principalTable: "Rooms",
                principalColumn: "Room_Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Locations_LocationId",
                table: "Rooms",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Warehouses_Warehouse_Id",
                table: "Rooms",
                column: "Warehouse_Id",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Locations_LocationId",
                table: "Shelves",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Racks_Row_Id",
                table: "Shelves",
                column: "Row_Id",
                principalTable: "Racks",
                principalColumn: "Rack_Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Bins_Bin_Id",
                table: "Stocks",
                column: "Bin_Id",
                principalTable: "Bins",
                principalColumn: "Bin_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bins_Locations_LocationId",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_Shelves_Shelf_Id",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Locations_LocationId",
                table: "Racks");

            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Rooms_Room_Id",
                table: "Racks");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Locations_LocationId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Warehouses_Warehouse_Id",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Locations_LocationId",
                table: "Shelves");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Racks_Row_Id",
                table: "Shelves");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Bins_Bin_Id",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shelves",
                table: "Shelves");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Racks",
                table: "Racks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bins",
                table: "Bins");

            migrationBuilder.RenameTable(
                name: "Shelves",
                newName: "Shelf");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "Racks",
                newName: "Rack");

            migrationBuilder.RenameTable(
                name: "Bins",
                newName: "Bin");

            migrationBuilder.RenameIndex(
                name: "IX_Shelves_Row_Id",
                table: "Shelf",
                newName: "IX_Shelf_Row_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Shelves_LocationId",
                table: "Shelf",
                newName: "IX_Shelf_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_Warehouse_Id",
                table: "Room",
                newName: "IX_Room_Warehouse_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_LocationId",
                table: "Room",
                newName: "IX_Room_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Racks_Room_Id",
                table: "Rack",
                newName: "IX_Rack_Room_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Racks_LocationId",
                table: "Rack",
                newName: "IX_Rack_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Bins_Shelf_Id",
                table: "Bin",
                newName: "IX_Bin_Shelf_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Bins_LocationId",
                table: "Bin",
                newName: "IX_Bin_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shelf",
                table: "Shelf",
                column: "Shelf_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Room_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rack",
                table: "Rack",
                column: "Rack_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bin",
                table: "Bin",
                column: "Bin_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bin_Locations_LocationId",
                table: "Bin",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bin_Shelf_Shelf_Id",
                table: "Bin",
                column: "Shelf_Id",
                principalTable: "Shelf",
                principalColumn: "Shelf_Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Rack_Locations_LocationId",
                table: "Rack",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Rack_Room_Room_Id",
                table: "Rack",
                column: "Room_Id",
                principalTable: "Room",
                principalColumn: "Room_Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Locations_LocationId",
                table: "Room",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Warehouses_Warehouse_Id",
                table: "Room",
                column: "Warehouse_Id",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelf_Locations_LocationId",
                table: "Shelf",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelf_Rack_Row_Id",
                table: "Shelf",
                column: "Row_Id",
                principalTable: "Rack",
                principalColumn: "Rack_Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Bin_Bin_Id",
                table: "Stocks",
                column: "Bin_Id",
                principalTable: "Bin",
                principalColumn: "Bin_Id");
        }
    }
}
