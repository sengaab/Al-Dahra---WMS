
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class updatelocationFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =====================================================
            // 1. Remove old Bin -> Warehouse relationship
            // =====================================================

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_Warehouses_WarehouseId",
                table: "Bins");

            migrationBuilder.DropIndex(
                name: "IX_Locations_BinId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Bins_WarehouseId",
                table: "Bins");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Bins");


            // =====================================================
            // 2. Add WarehouseId to Locations
            //
            // IMPORTANT:
            // Nullable temporarily so existing rows can be
            // populated before adding the FK.
            // =====================================================

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Locations",
                type: "integer",
                nullable: true);


            // =====================================================
            // 3. Populate Locations.WarehouseId
            //
            // Location
            //    -> Bin
            //       -> Partition
            //          -> Warehouse
            //
            // Bin itself no longer has WarehouseId.
            // =====================================================

            migrationBuilder.Sql("""
                UPDATE "Locations" AS l
                SET "WarehouseId" = p."WarehouseId"
                FROM "Bins" AS b
                INNER JOIN "Partitions" AS p
                    ON b."PartitionId" = p."PartitionId"
                WHERE l."BinId" = b."Bin_Id";
            """);


            // =====================================================
            // 4. Make sure no Location was left without WarehouseId
            // =====================================================

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM "Locations"
                        WHERE "WarehouseId" IS NULL
                    ) THEN
                        RAISE EXCEPTION
                            'Cannot migrate Locations: some Locations could not be mapped to a Warehouse through Bin -> Partition.';
                    END IF;
                END $$;
            """);


            // =====================================================
            // 5. Change WarehouseId to NOT NULL
            // =====================================================

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "Locations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);


            // =====================================================
            // 6. Unique Bin -> Location relationship
            // =====================================================

            migrationBuilder.CreateIndex(
                name: "IX_Locations_BinId",
                table: "Locations",
                column: "BinId",
                unique: true);


            // =====================================================
            // 7. Index Location -> Warehouse
            // =====================================================

            migrationBuilder.CreateIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations",
                column: "WarehouseId");


            // =====================================================
            // 8. Add Location -> Warehouse FK
            // =====================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // =====================================================
            // 1. Remove Location -> Warehouse FK
            // =====================================================

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations");


            // =====================================================
            // 2. Remove Location indexes
            // =====================================================

            migrationBuilder.DropIndex(
                name: "IX_Locations_BinId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations");


            // =====================================================
            // 3. Remove WarehouseId from Locations
            // =====================================================

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Locations");


            // =====================================================
            // 4. Restore WarehouseId to Bins
            // =====================================================

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Bins",
                type: "integer",
                nullable: false,
                defaultValue: 0);


            // =====================================================
            // 5. Restore Bin -> Warehouse relationship
            //
            // Populate Bin.WarehouseId from:
            // Bin -> Partition -> Warehouse
            // =====================================================

            migrationBuilder.Sql("""
                UPDATE "Bins" AS b
                SET "WarehouseId" = p."WarehouseId"
                FROM "Partitions" AS p
                WHERE b."PartitionId" = p."PartitionId";
            """);


            // =====================================================
            // 6. Restore Bin -> Location non-unique index
            // =====================================================

            migrationBuilder.CreateIndex(
                name: "IX_Locations_BinId",
                table: "Locations",
                column: "BinId");


            // =====================================================
            // 7. Restore Bin -> Warehouse index
            // =====================================================

            migrationBuilder.CreateIndex(
                name: "IX_Bins_WarehouseId",
                table: "Bins",
                column: "WarehouseId");


            // =====================================================
            // 8. Restore Bin -> Warehouse FK
            // =====================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_Warehouses_WarehouseId",
                table: "Bins",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
