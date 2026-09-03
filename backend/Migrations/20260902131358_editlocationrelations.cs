using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class editlocationrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =========================================================
            // DROP OLD LOCATION FOREIGN KEYS
            // =========================================================

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_Locations_LocationId",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Locations_LocationId",
                table: "Racks");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Locations_LocationId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Locations_LocationId",
                table: "Shelves");


            // =========================================================
            // DROP OLD SHELF -> RACK FOREIGN KEY
            // Actual FK name in PostgreSQL:
            // Shelves_Row_Id_fkey
            // =========================================================

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_constraint
                        WHERE conname = 'Shelves_Row_Id_fkey'
                          AND conrelid = '"Shelves"'::regclass
                    ) THEN
                        ALTER TABLE "Shelves"
                        DROP CONSTRAINT "Shelves_Row_Id_fkey";
                    END IF;
                END
                $$;
                """);


            // =========================================================
            // DROP OLD INDEXES
            // =========================================================

            migrationBuilder.DropIndex(
                name: "IX_Shelves_LocationId",
                table: "Shelves");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_LocationId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Racks_LocationId",
                table: "Racks");

            migrationBuilder.DropIndex(
                name: "IX_Bins_LocationId",
                table: "Bins");


            // =========================================================
            // DROP OLD SHELF ROW INDEX
            // =========================================================

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_indexes
                        WHERE schemaname = 'public'
                          AND tablename = 'Shelves'
                          AND indexname = 'IX_Shelves_Row_Id'
                    ) THEN
                        DROP INDEX "IX_Shelves_Row_Id";
                    END IF;
                END
                $$;
                """);


            // =========================================================
            // DROP OLD LOCATION COLUMNS
            // =========================================================

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Shelves");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Racks");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Bins");


            // =========================================================
            // RENAME SHELF Row_Id -> Rack_Id
            // =========================================================

            migrationBuilder.RenameColumn(
                name: "Row_Id",
                table: "Shelves",
                newName: "Rack_Id");


            // =========================================================
            // CREATE NEW SHELF -> RACK INDEX
            // =========================================================

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_Rack_Id",
                table: "Shelves",
                column: "Rack_Id");


            // =========================================================
            // ADD LOCATION REFERENCES
            // =========================================================

            migrationBuilder.AddColumn<int>(
                name: "BinId",
                table: "Locations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RackId",
                table: "Locations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Locations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShelfId",
                table: "Locations",
                type: "integer",
                nullable: true);


            // =========================================================
            // LOCATION INDEXES
            // =========================================================

            migrationBuilder.CreateIndex(
                name: "IX_Locations_BinId",
                table: "Locations",
                column: "BinId",
                unique: true,
                filter: "\"BinId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_RackId",
                table: "Locations",
                column: "RackId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_RoomId",
                table: "Locations",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ShelfId",
                table: "Locations",
                column: "ShelfId");


            // =========================================================
            // LOCATION -> BIN
            // One Location <-> One Bin
            // FK is Location.BinId
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Bins_BinId",
                table: "Locations",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "Bin_Id",
                onDelete: ReferentialAction.SetNull);


            // =========================================================
            // LOCATION -> RACK
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Racks_RackId",
                table: "Locations",
                column: "RackId",
                principalTable: "Racks",
                principalColumn: "Rack_Id",
                onDelete: ReferentialAction.SetNull);


            // =========================================================
            // LOCATION -> ROOM
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Rooms_RoomId",
                table: "Locations",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Room_Id",
                onDelete: ReferentialAction.SetNull);


            // =========================================================
            // LOCATION -> SHELF
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Shelves_ShelfId",
                table: "Locations",
                column: "ShelfId",
                principalTable: "Shelves",
                principalColumn: "Shelf_Id",
                onDelete: ReferentialAction.SetNull);


            // =========================================================
            // SHELF -> RACK
            // Shelf.Rack_Id -> Rack.Rack_Id
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Racks_Rack_Id",
                table: "Shelves",
                column: "Rack_Id",
                principalTable: "Racks",
                principalColumn: "Rack_Id",
                onDelete: ReferentialAction.SetNull);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // =========================================================
            // DROP NEW FOREIGN KEYS
            // =========================================================

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Bins_BinId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Racks_RackId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Rooms_RoomId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Shelves_ShelfId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Racks_Rack_Id",
                table: "Shelves");


            // =========================================================
            // DROP NEW INDEXES
            // =========================================================

            migrationBuilder.DropIndex(
                name: "IX_Locations_BinId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_RackId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_RoomId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ShelfId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Shelves_Rack_Id",
                table: "Shelves");


            // =========================================================
            // DROP NEW LOCATION COLUMNS
            // =========================================================

            migrationBuilder.DropColumn(
                name: "BinId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RackId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ShelfId",
                table: "Locations");


            // =========================================================
            // RENAME Rack_Id -> Row_Id
            // =========================================================

            migrationBuilder.RenameColumn(
                name: "Rack_Id",
                table: "Shelves",
                newName: "Row_Id");


            // =========================================================
            // CREATE OLD ROW INDEX
            // =========================================================

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_Row_Id",
                table: "Shelves",
                column: "Row_Id");


            // =========================================================
            // ADD OLD LOCATION COLUMNS
            // =========================================================

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Shelves",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Rooms",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Racks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Bins",
                type: "integer",
                nullable: true);


            // =========================================================
            // OLD LOCATION INDEXES
            // =========================================================

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_LocationId",
                table: "Shelves",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LocationId",
                table: "Rooms",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_LocationId",
                table: "Racks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_LocationId",
                table: "Bins",
                column: "LocationId");


            // =========================================================
            // OLD BIN -> LOCATION
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_Locations_LocationId",
                table: "Bins",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);


            // =========================================================
            // OLD RACK -> LOCATION
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Locations_LocationId",
                table: "Racks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);


            // =========================================================
            // OLD ROOM -> LOCATION
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Locations_LocationId",
                table: "Rooms",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);


            // =========================================================
            // OLD SHELF -> LOCATION
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Locations_LocationId",
                table: "Shelves",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);


            // =========================================================
            // OLD SHELF -> RACK
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "Shelves_Row_Id_fkey",
                table: "Shelves",
                column: "Row_Id",
                principalTable: "Racks",
                principalColumn: "Rack_Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}