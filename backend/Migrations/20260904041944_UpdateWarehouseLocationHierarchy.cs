using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWarehouseLocationHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =========================
            // LOCATIONS
            // =========================

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ParentLocationId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ParentLocationId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "Locations",
                newName: "BinId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations",
                newName: "IX_Locations_BinId");


            // =========================
            // PURCHASE ORDERS
            // =========================

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "PurchaseOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(
                        1,
                        1,
                        1,
                        0,
                        0,
                        0,
                        0,
                        DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));


            // =========================
            // PARTITIONS
            // =========================

            migrationBuilder.CreateTable(
                name: "Partitions",
                columns: table => new
                {
                    PartitionId = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    WarehouseId = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    Code = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false),

                    Name = table.Column<string>(
                        type: "character varying(150)",
                        maxLength: 150,
                        nullable: false),

                    Description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true),

                    IsActive = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: true),

                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false),

                    UpdatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Partitions",
                        x => x.PartitionId);

                    table.ForeignKey(
                        name: "FK_Partitions_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });


            // =========================
            // BINS
            // =========================

            migrationBuilder.CreateTable(
                name: "Bins",
                columns: table => new
                {
                    Bin_Id = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    WarehouseId = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    PartitionId = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    Bin_Name = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false),

                    Bin_Code = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true),

                    Bin_Description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true),

                    IsActive = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Bins",
                        x => x.Bin_Id);

                    table.ForeignKey(
                        name: "FK_Bins_Partitions_PartitionId",
                        column: x => x.PartitionId,
                        principalTable: "Partitions",
                        principalColumn: "PartitionId",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_Bins_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });


            // =========================
            // BINS INDEXES
            // =========================

            migrationBuilder.CreateIndex(
                name: "IX_Bins_Bin_Code",
                table: "Bins",
                column: "Bin_Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bins_PartitionId",
                table: "Bins",
                column: "PartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_WarehouseId",
                table: "Bins",
                column: "WarehouseId");


            // =========================
            // PARTITIONS INDEX
            // =========================

            migrationBuilder.CreateIndex(
                name: "IX_Partitions_WarehouseId_Code",
                table: "Partitions",
                columns: new[]
                {
                    "WarehouseId",
                    "Code"
                },
                unique: true);


            // =========================
            // LOCATION -> BIN
            // =========================

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Bins_BinId",
                table: "Locations",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "Bin_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // =========================
            // LOCATION -> BIN
            // =========================

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Bins_BinId",
                table: "Locations");


            // =========================
            // DROP BINS / PARTITIONS
            // =========================

            migrationBuilder.DropTable(
                name: "Bins");

            migrationBuilder.DropTable(
                name: "Partitions");


            // =========================
            // PURCHASE ORDERS
            // =========================

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PurchaseOrders");


            // =========================
            // LOCATIONS
            // =========================

            migrationBuilder.RenameColumn(
                name: "BinId",
                table: "Locations",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_BinId",
                table: "Locations",
                newName: "IX_Locations_WarehouseId");

            migrationBuilder.AddColumn<int>(
                name: "ParentLocationId",
                table: "Locations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId");

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