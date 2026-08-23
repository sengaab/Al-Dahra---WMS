using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace whm.Migrations
{
    public partial class UpdateDatabaseModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =========================================================
            // PRODUCTS
            // =========================================================

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Products");


            // =========================================================
            // TRANSACTIONS
            // =========================================================

            migrationBuilder.DropTable(
                name: "Transactions");


            // =========================================================
            // STOCKS
            // =========================================================

            migrationBuilder.RenameColumn(
                name: "StockStatue",
                table: "Stocks",
                newName: "StockStatus");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryStatus",
                table: "Stocks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MinimumStock",
                table: "Stocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReservedQuantity",
                table: "Stocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // IMPORTANT:
            // Nullable because old Stocks already exist
            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "Stocks",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            // IMPORTANT:
            // Nullable because old records do not have UnitId
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Stocks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Stocks",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);


            // =========================================================
            // REPORT SCHEDULE
            // =========================================================

            migrationBuilder.AlterColumn<string>(
                name: "ReportType",
                table: "ReportSchedules",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");


            // =========================================================
            // PRODUCT UNIT
            // =========================================================

            migrationBuilder.AddColumn<int>(
                name: "Unit_Id",
                table: "Products",
                type: "integer",
                nullable: true);


            // =========================================================
            // ALIASES
            // =========================================================

            migrationBuilder.CreateTable(
                name: "Aliases",
                columns: table => new
                {
                    AliasId = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    AliasName = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false),

                    ProductId = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    IsActive = table.Column<bool>(
                        type: "boolean",
                        nullable: false),

                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Aliases",
                        x => x.AliasId);

                    table.ForeignKey(
                        name: "FK_Aliases_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });


            // =========================================================
            // OPERATIONS
            // =========================================================

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Operation_Id = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    OperationType = table.Column<string>(
                        type: "text",
                        nullable: false),

                    Product_Id = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    Quantity = table.Column<decimal>(
                        type: "numeric(18,3)",
                        precision: 18,
                        scale: 3,
                        nullable: false),

                    Unit_Id = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    FromBinId = table.Column<int>(
                        type: "integer",
                        nullable: true),

                    ToBinId = table.Column<int>(
                        type: "integer",
                        nullable: true),

                    User_Id = table.Column<Guid>(
                        type: "uuid",
                        nullable: false),

                    Notes = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true),

                    CreateAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Operations",
                        x => x.Operation_Id);

                    table.ForeignKey(
                        name: "FK_Operations_Bins_FromBinId",
                        column: x => x.FromBinId,
                        principalTable: "Bins",
                        principalColumn: "Bin_Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Operations_Bins_ToBinId",
                        column: x => x.ToBinId,
                        principalTable: "Bins",
                        principalColumn: "Bin_Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Operations_Products_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Operations_Units_Unit_Id",
                        column: x => x.Unit_Id,
                        principalTable: "Units",
                        principalColumn: "Unit_Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Operations_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });


            // =========================================================
            // SUPPLIERS
            // =========================================================

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    SupplierCode = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false),

                    SupplierName = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false),

                    ContactPerson = table.Column<string>(
                        type: "character varying(150)",
                        maxLength: 150,
                        nullable: true),

                    Phone = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true),

                    Email = table.Column<string>(
                        type: "character varying(150)",
                        maxLength: 150,
                        nullable: true),

                    Address = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true),

                    Country = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true),

                    TaxNumber = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: true),

                    PaymentTerms = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true),

                    Currency = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: true),

                    Status = table.Column<string>(
                        type: "text",
                        nullable: false),

                    Notes = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true),

                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false),

                    UpdatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Suppliers",
                        x => x.SupplierId);
                });


            // =========================================================
            // ORDERS
            // =========================================================

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    OrderNumber = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false),

                    SupplierId = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    OrderDate = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false),

                    ExpectedDate = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true),

                    Status = table.Column<string>(
                        type: "text",
                        nullable: false),

                    Priority = table.Column<string>(
                        type: "text",
                        nullable: false),

                    WarehouseId = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    CreatedBy = table.Column<Guid>(
                        type: "uuid",
                        nullable: false),

                    ApprovedBy = table.Column<Guid>(
                        type: "uuid",
                        nullable: true),

                    Notes = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true),

                    Subtotal = table.Column<decimal>(
                        type: "numeric(18,2)",
                        precision: 18,
                        scale: 2,
                        nullable: false),

                    TaxAmount = table.Column<decimal>(
                        type: "numeric(18,2)",
                        precision: 18,
                        scale: 2,
                        nullable: false),

                    TotalAmount = table.Column<decimal>(
                        type: "numeric(18,2)",
                        precision: 18,
                        scale: 2,
                        nullable: false),

                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false),

                    UpdatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Orders",
                        x => x.OrderId);

                    table.ForeignKey(
                        name: "FK_Orders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Orders_Users_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Orders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Orders_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Warehouse_Id",
                        onDelete: ReferentialAction.Restrict);
                });


            // =========================================================
            // ORDER ITEMS
            // =========================================================

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    OrderId = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    ProductId = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    Quantity = table.Column<decimal>(
                        type: "numeric(18,3)",
                        precision: 18,
                        scale: 3,
                        nullable: false),

                    UnitPrice = table.Column<decimal>(
                        type: "numeric(18,2)",
                        precision: 18,
                        scale: 2,
                        nullable: false),

                    TaxRate = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: false),

                    TotalPrice = table.Column<decimal>(
                        type: "numeric(18,2)",
                        precision: 18,
                        scale: 2,
                        nullable: false),

                    ReceivedQuantity = table.Column<decimal>(
                        type: "numeric(18,3)",
                        precision: 18,
                        scale: 3,
                        nullable: false),

                    Notes = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_OrderItems",
                        x => x.OrderItemId);

                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });


            // =========================================================
            // FIX OLD STOCK CODES
            // =========================================================

            migrationBuilder.Sql("""
                UPDATE "Stocks"
                SET "StockCode" = 'STK-' || "Stock_Id"
                WHERE "StockCode" IS NULL
                   OR "StockCode" = '';

                WITH duplicates AS
                (
                    SELECT
                        "Stock_Id",
                        ROW_NUMBER() OVER (
                            PARTITION BY "StockCode"
                            ORDER BY "Stock_Id"
                        ) AS rn
                    FROM "Stocks"
                    WHERE "StockCode" IS NOT NULL
                      AND "StockCode" <> ''
                )
                UPDATE "Stocks" s
                SET "StockCode" = 'STK-' || s."Stock_Id"
                FROM duplicates d
                WHERE s."Stock_Id" = d."Stock_Id"
                  AND d.rn > 1;
            """);


            // =========================================================
            // FIX OLD PRODUCT SKU
            // =========================================================

            migrationBuilder.Sql("""
                WITH duplicates AS
                (
                    SELECT
                        "ProductId",
                        ROW_NUMBER() OVER (
                            PARTITION BY "SKU"
                            ORDER BY "ProductId"
                        ) AS rn
                    FROM "Products"
                    WHERE "SKU" IS NOT NULL
                      AND "SKU" <> ''
                )
                UPDATE "Products" p
                SET "SKU" = 'SKU-' || p."ProductId"
                FROM duplicates d
                WHERE p."ProductId" = d."ProductId"
                  AND d.rn > 1;

                UPDATE "Products"
                SET "SKU" = 'SKU-' || "ProductId"
                WHERE "SKU" IS NULL
                   OR "SKU" = '';
            """);


            // =========================================================
            // FIX OLD PRODUCT QR VALUE
            // =========================================================

            migrationBuilder.Sql("""
                WITH duplicates AS
                (
                    SELECT
                        "ProductId",
                        ROW_NUMBER() OVER (
                            PARTITION BY "QRValue"
                            ORDER BY "ProductId"
                        ) AS rn
                    FROM "Products"
                    WHERE "QRValue" IS NOT NULL
                      AND "QRValue" <> ''
                )
                UPDATE "Products" p
                SET "QRValue" = 'QR-' || p."ProductId"
                FROM duplicates d
                WHERE p."ProductId" = d."ProductId"
                  AND d.rn > 1;

                UPDATE "Products"
                SET "QRValue" = 'QR-' || "ProductId"
                WHERE "QRValue" IS NULL
                   OR "QRValue" = '';
            """);


            // =========================================================
            // INDEXES
            // =========================================================

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_StockCode",
                table: "Stocks",
                column: "StockCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_UnitId",
                table: "Stocks",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_QRValue",
                table: "Products",
                column: "QRValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Unit_Id",
                table: "Products",
                column: "Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Aliases_ProductId_AliasName",
                table: "Aliases",
                columns: new[] { "ProductId", "AliasName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_FromBinId",
                table: "Operations",
                column: "FromBinId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Product_Id",
                table: "Operations",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ToBinId",
                table: "Operations",
                column: "ToBinId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Unit_Id",
                table: "Operations",
                column: "Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_User_Id",
                table: "Operations",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApprovedBy",
                table: "Orders",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedBy",
                table: "Orders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SupplierId",
                table: "Orders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WarehouseId",
                table: "Orders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierCode",
                table: "Suppliers",
                column: "SupplierCode",
                unique: true);


            // =========================================================
            // FOREIGN KEYS
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_Unit_Id",
                table: "Products",
                column: "Unit_Id",
                principalTable: "Units",
                principalColumn: "Unit_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Units_UnitId",
                table: "Stocks",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Unit_Id",
                onDelete: ReferentialAction.Restrict);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // =========================================================
            // FOREIGN KEYS
            // =========================================================

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_Unit_Id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Units_UnitId",
                table: "Stocks");


            // =========================================================
            // TABLES
            // =========================================================

            migrationBuilder.DropTable(
                name: "Aliases");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Suppliers");


            // =========================================================
            // INDEXES
            // =========================================================

            migrationBuilder.DropIndex(
                name: "IX_Stocks_StockCode",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_UnitId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Products_QRValue",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SKU",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Unit_Id",
                table: "Products");


            // =========================================================
            // STOCK COLUMNS
            // =========================================================

            migrationBuilder.DropColumn(
                name: "DeliveryStatus",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "ReservedQuantity",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Stocks");


            // =========================================================
            // PRODUCT UNIT
            // =========================================================

            migrationBuilder.DropColumn(
                name: "Unit_Id",
                table: "Products");


            // =========================================================
            // STOCK STATUS
            // =========================================================

            migrationBuilder.RenameColumn(
                name: "StockStatus",
                table: "Stocks",
                newName: "StockStatue");


            // =========================================================
            // REPORT TYPE
            // =========================================================

            migrationBuilder.AlterColumn<int>(
                name: "ReportType",
                table: "ReportSchedules",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");


            // =========================================================
            // OLD PRODUCT COLUMNS
            // =========================================================

            migrationBuilder.AddColumn<int>(
                name: "MinimumStock",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);


            // =========================================================
            // TRANSACTIONS
            // =========================================================

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    transaction_Id = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    FromBinId = table.Column<int>(
                        type: "integer",
                        nullable: true),

                    Product_Id = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    ToBinId = table.Column<int>(
                        type: "integer",
                        nullable: true),

                    Unit_Id = table.Column<int>(
                        type: "integer",
                        nullable: false),

                    User_Id = table.Column<Guid>(
                        type: "uuid",
                        nullable: false),

                    CreateAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false),

                    Notes = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true),

                    Quantity = table.Column<decimal>(
                        type: "numeric(18,3)",
                        precision: 18,
                        scale: 3,
                        nullable: false),

                    TransactionType = table.Column<string>(
                        type: "text",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Transactions",
                        x => x.transaction_Id);

                    table.ForeignKey(
                        name: "FK_Transactions_Bins_FromBinId",
                        column: x => x.FromBinId,
                        principalTable: "Bins",
                        principalColumn: "Bin_Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Transactions_Bins_ToBinId",
                        column: x => x.ToBinId,
                        principalTable: "Bins",
                        principalColumn: "Bin_Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Transactions_Products_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Transactions_Units_Unit_Id",
                        column: x => x.Unit_Id,
                        principalTable: "Units",
                        principalColumn: "Unit_Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Transactions_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });


            // =========================================================
            // OLD INDEX
            // =========================================================

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId",
                table: "Products",
                column: "UnitId");


            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromBinId",
                table: "Transactions",
                column: "FromBinId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Product_Id",
                table: "Transactions",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToBinId",
                table: "Transactions",
                column: "ToBinId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Unit_Id",
                table: "Transactions",
                column: "Unit_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_User_Id",
                table: "Transactions",
                column: "User_Id");


            // =========================================================
            // OLD FOREIGN KEYS
            // =========================================================

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Unit_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}