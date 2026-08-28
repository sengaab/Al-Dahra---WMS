using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Migrations
{
    /// <inheritdoc />
    public partial class FixBarcodeScanRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_UserId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_BarcodeScans_Locations_LocationId",
                table: "BarcodeScans");

            migrationBuilder.DropForeignKey(
                name: "FK_BarcodeScans_Products_ProductId",
                table: "BarcodeScans");

            migrationBuilder.DropForeignKey(
                name: "FK_BarcodeScans_Stocks_StockId",
                table: "BarcodeScans");

            migrationBuilder.DropForeignKey(
                name: "FK_BarcodeScans_Users_ScannedBy",
                table: "BarcodeScans");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Users_InspectedBy",
                table: "Inspections");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_PickItems_Locations_LocationId",
                table: "PickItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PickItems_Products_ProductId",
                table: "PickItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PickItems_Stocks_StockId",
                table: "PickItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PickLists_StockRequests_RequestId",
                table: "PickLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PickLists_Users_AssignedTo",
                table: "PickLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PickLists_Warehouses_WarehouseId",
                table: "PickLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Products_ProductId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Sites_SiteId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Users_ApprovedBy",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Users_CreatedBy",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PutawayItems_Locations_LocationId",
                table: "PutawayItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PutawayItems_Products_ProductId",
                table: "PutawayItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PutawayItems_ReceiptItems_ReceiptItemId",
                table: "PutawayItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PutawayItems_Stocks_StockId",
                table: "PutawayItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Putaways_Receipts_ReceiptId",
                table: "Putaways");

            migrationBuilder.DropForeignKey(
                name: "FK_Putaways_Users_CreatedBy",
                table: "Putaways");

            migrationBuilder.DropForeignKey(
                name: "FK_Putaways_Warehouses_WarehouseId",
                table: "Putaways");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_Products_ProductId",
                table: "ReceiptItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_PurchaseOrderItems_PurchaseOrderItemId",
                table: "ReceiptItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_PurchaseOrders_PurchaseOrderId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Users_ReceivedBy",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Warehouses_WarehouseId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_CreatedBy",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_StockRequestItems_RequestItemId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Stocks_StockId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_ReservedBy",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Products_ProductId",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Stocks_StockId",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Users_ApprovedBy",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Users_CreatedBy",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCountItems_Products_ProductId",
                table: "StockCountItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCountItems_Stocks_StockId",
                table: "StockCountItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCounts_Locations_LocationId",
                table: "StockCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCounts_Users_ApprovedBy",
                table: "StockCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCounts_Users_CreatedBy",
                table: "StockCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCounts_Warehouses_WarehouseId",
                table: "StockCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssueItems_Products_ProductId",
                table: "StockIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssueItems_Stocks_StockId",
                table: "StockIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_Departments_DepartmentId",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_PickLists_PickListId",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_StockRequests_RequestId",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_Users_IssuedBy",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_Warehouses_WarehouseId",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequestItems_Products_ProductId",
                table: "StockRequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequests_Departments_DepartmentId",
                table: "StockRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequests_Sites_SiteId",
                table: "StockRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequests_Users_ApprovedBy",
                table: "StockRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequests_Users_RequestedBy",
                table: "StockRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturnItems_Products_ProductId",
                table: "StockReturnItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturnItems_Stocks_StockId",
                table: "StockReturnItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturns_Departments_DepartmentId",
                table: "StockReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturns_StockIssues_IssueId",
                table: "StockReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturns_Users_ReturnedBy",
                table: "StockReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturns_Warehouses_WarehouseId",
                table: "StockReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Locations_LocationId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Products_ProductId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Warehouses_WarehouseId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Locations_DestinationLocationId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Locations_SourceLocationId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Products_ProductId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Stocks_StockId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Users_PerformedBy",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_Locations_DestinationLocationId",
                table: "StockTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_Locations_SourceLocationId",
                table: "StockTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_Products_ProductId",
                table: "StockTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_Stocks_SourceStockId",
                table: "StockTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Users_ApprovedBy",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Users_RequestedBy",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Warehouses_DestinationWarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Warehouses_SourceWarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierProducts_Products_ProductId",
                table: "SupplierProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierProducts_Suppliers_SupplierId",
                table: "SupplierProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Sites_SiteId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_SiteId_Code",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeCode",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Units_Abbreviation",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_Name",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Products_QRValue",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Locations_WarehouseId_Code",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Name",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Warehouses",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Suppliers",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Sites",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "QRValue",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Locations",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Departments",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Code",
                table: "Warehouses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_SiteId",
                table: "Warehouses",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeCode",
                table: "Users",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Code",
                table: "Locations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_UserId",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BarcodeScans_Locations_LocationId",
                table: "BarcodeScans",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BarcodeScans_Products_ProductId",
                table: "BarcodeScans",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BarcodeScans_Stocks_StockId",
                table: "BarcodeScans",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BarcodeScans_Users_ScannedBy",
                table: "BarcodeScans",
                column: "ScannedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Users_InspectedBy",
                table: "Inspections",
                column: "InspectedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_PickItems_Locations_LocationId",
                table: "PickItems",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PickItems_Products_ProductId",
                table: "PickItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PickItems_Stocks_StockId",
                table: "PickItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PickLists_StockRequests_RequestId",
                table: "PickLists",
                column: "RequestId",
                principalTable: "StockRequests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PickLists_Users_AssignedTo",
                table: "PickLists",
                column: "AssignedTo",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PickLists_Warehouses_WarehouseId",
                table: "PickLists",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Products_ProductId",
                table: "PurchaseOrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Sites_SiteId",
                table: "PurchaseOrders",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Users_ApprovedBy",
                table: "PurchaseOrders",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Users_CreatedBy",
                table: "PurchaseOrders",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PutawayItems_Locations_LocationId",
                table: "PutawayItems",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PutawayItems_Products_ProductId",
                table: "PutawayItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PutawayItems_ReceiptItems_ReceiptItemId",
                table: "PutawayItems",
                column: "ReceiptItemId",
                principalTable: "ReceiptItems",
                principalColumn: "ReceiptItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PutawayItems_Stocks_StockId",
                table: "PutawayItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Putaways_Receipts_ReceiptId",
                table: "Putaways",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "ReceiptId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Putaways_Users_CreatedBy",
                table: "Putaways",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Putaways_Warehouses_WarehouseId",
                table: "Putaways",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_Products_ProductId",
                table: "ReceiptItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_PurchaseOrderItems_PurchaseOrderItemId",
                table: "ReceiptItems",
                column: "PurchaseOrderItemId",
                principalTable: "PurchaseOrderItems",
                principalColumn: "PurchaseOrderItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_PurchaseOrders_PurchaseOrderId",
                table: "Receipts",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "PurchaseOrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Users_ReceivedBy",
                table: "Receipts",
                column: "ReceivedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Warehouses_WarehouseId",
                table: "Receipts",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_CreatedBy",
                table: "Reports",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_StockRequestItems_RequestItemId",
                table: "Reservations",
                column: "RequestItemId",
                principalTable: "StockRequestItems",
                principalColumn: "RequestItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Stocks_StockId",
                table: "Reservations",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_ReservedBy",
                table: "Reservations",
                column: "ReservedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Products_ProductId",
                table: "StockAdjustments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Stocks_StockId",
                table: "StockAdjustments",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Users_ApprovedBy",
                table: "StockAdjustments",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Users_CreatedBy",
                table: "StockAdjustments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCountItems_Products_ProductId",
                table: "StockCountItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCountItems_Stocks_StockId",
                table: "StockCountItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCounts_Locations_LocationId",
                table: "StockCounts",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCounts_Users_ApprovedBy",
                table: "StockCounts",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCounts_Users_CreatedBy",
                table: "StockCounts",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCounts_Warehouses_WarehouseId",
                table: "StockCounts",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssueItems_Products_ProductId",
                table: "StockIssueItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssueItems_Stocks_StockId",
                table: "StockIssueItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_Departments_DepartmentId",
                table: "StockIssues",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_PickLists_PickListId",
                table: "StockIssues",
                column: "PickListId",
                principalTable: "PickLists",
                principalColumn: "PickListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_StockRequests_RequestId",
                table: "StockIssues",
                column: "RequestId",
                principalTable: "StockRequests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_Users_IssuedBy",
                table: "StockIssues",
                column: "IssuedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_Warehouses_WarehouseId",
                table: "StockIssues",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequestItems_Products_ProductId",
                table: "StockRequestItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequests_Departments_DepartmentId",
                table: "StockRequests",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequests_Sites_SiteId",
                table: "StockRequests",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequests_Users_ApprovedBy",
                table: "StockRequests",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequests_Users_RequestedBy",
                table: "StockRequests",
                column: "RequestedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturnItems_Products_ProductId",
                table: "StockReturnItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturnItems_Stocks_StockId",
                table: "StockReturnItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturns_Departments_DepartmentId",
                table: "StockReturns",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturns_StockIssues_IssueId",
                table: "StockReturns",
                column: "IssueId",
                principalTable: "StockIssues",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturns_Users_ReturnedBy",
                table: "StockReturns",
                column: "ReturnedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturns_Warehouses_WarehouseId",
                table: "StockReturns",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Locations_LocationId",
                table: "Stocks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Products_ProductId",
                table: "Stocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Warehouses_WarehouseId",
                table: "Stocks",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Locations_DestinationLocationId",
                table: "StockTransactions",
                column: "DestinationLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Locations_SourceLocationId",
                table: "StockTransactions",
                column: "SourceLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Products_ProductId",
                table: "StockTransactions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Stocks_StockId",
                table: "StockTransactions",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Users_PerformedBy",
                table: "StockTransactions",
                column: "PerformedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_Locations_DestinationLocationId",
                table: "StockTransferItems",
                column: "DestinationLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_Locations_SourceLocationId",
                table: "StockTransferItems",
                column: "SourceLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_Products_ProductId",
                table: "StockTransferItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_Stocks_SourceStockId",
                table: "StockTransferItems",
                column: "SourceStockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Users_ApprovedBy",
                table: "StockTransfers",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Users_RequestedBy",
                table: "StockTransfers",
                column: "RequestedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Warehouses_DestinationWarehouseId",
                table: "StockTransfers",
                column: "DestinationWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Warehouses_SourceWarehouseId",
                table: "StockTransfers",
                column: "SourceWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierProducts_Products_ProductId",
                table: "SupplierProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierProducts_Suppliers_SupplierId",
                table: "SupplierProducts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Sites_SiteId",
                table: "Warehouses",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_UserId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_BarcodeScans_Locations_LocationId",
                table: "BarcodeScans");

            migrationBuilder.DropForeignKey(
                name: "FK_BarcodeScans_Products_ProductId",
                table: "BarcodeScans");

            migrationBuilder.DropForeignKey(
                name: "FK_BarcodeScans_Stocks_StockId",
                table: "BarcodeScans");

            migrationBuilder.DropForeignKey(
                name: "FK_BarcodeScans_Users_ScannedBy",
                table: "BarcodeScans");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Users_InspectedBy",
                table: "Inspections");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentLocationId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Warehouses_WarehouseId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_PickItems_Locations_LocationId",
                table: "PickItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PickItems_Products_ProductId",
                table: "PickItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PickItems_Stocks_StockId",
                table: "PickItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PickLists_StockRequests_RequestId",
                table: "PickLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PickLists_Users_AssignedTo",
                table: "PickLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PickLists_Warehouses_WarehouseId",
                table: "PickLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Products_ProductId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Sites_SiteId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Users_ApprovedBy",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Users_CreatedBy",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PutawayItems_Locations_LocationId",
                table: "PutawayItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PutawayItems_Products_ProductId",
                table: "PutawayItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PutawayItems_ReceiptItems_ReceiptItemId",
                table: "PutawayItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PutawayItems_Stocks_StockId",
                table: "PutawayItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Putaways_Receipts_ReceiptId",
                table: "Putaways");

            migrationBuilder.DropForeignKey(
                name: "FK_Putaways_Users_CreatedBy",
                table: "Putaways");

            migrationBuilder.DropForeignKey(
                name: "FK_Putaways_Warehouses_WarehouseId",
                table: "Putaways");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_Products_ProductId",
                table: "ReceiptItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_PurchaseOrderItems_PurchaseOrderItemId",
                table: "ReceiptItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_PurchaseOrders_PurchaseOrderId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Users_ReceivedBy",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Warehouses_WarehouseId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_CreatedBy",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_StockRequestItems_RequestItemId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Stocks_StockId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_ReservedBy",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Products_ProductId",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Stocks_StockId",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Users_ApprovedBy",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Users_CreatedBy",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCountItems_Products_ProductId",
                table: "StockCountItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCountItems_Stocks_StockId",
                table: "StockCountItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCounts_Locations_LocationId",
                table: "StockCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCounts_Users_ApprovedBy",
                table: "StockCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCounts_Users_CreatedBy",
                table: "StockCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCounts_Warehouses_WarehouseId",
                table: "StockCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssueItems_Products_ProductId",
                table: "StockIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssueItems_Stocks_StockId",
                table: "StockIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_Departments_DepartmentId",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_PickLists_PickListId",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_StockRequests_RequestId",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_Users_IssuedBy",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIssues_Warehouses_WarehouseId",
                table: "StockIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequestItems_Products_ProductId",
                table: "StockRequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequests_Departments_DepartmentId",
                table: "StockRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequests_Sites_SiteId",
                table: "StockRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequests_Users_ApprovedBy",
                table: "StockRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequests_Users_RequestedBy",
                table: "StockRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturnItems_Products_ProductId",
                table: "StockReturnItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturnItems_Stocks_StockId",
                table: "StockReturnItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturns_Departments_DepartmentId",
                table: "StockReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturns_StockIssues_IssueId",
                table: "StockReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturns_Users_ReturnedBy",
                table: "StockReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockReturns_Warehouses_WarehouseId",
                table: "StockReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Locations_LocationId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Products_ProductId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Warehouses_WarehouseId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Locations_DestinationLocationId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Locations_SourceLocationId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Products_ProductId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Stocks_StockId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Users_PerformedBy",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_Locations_DestinationLocationId",
                table: "StockTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_Locations_SourceLocationId",
                table: "StockTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_Products_ProductId",
                table: "StockTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_Stocks_SourceStockId",
                table: "StockTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Users_ApprovedBy",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Users_RequestedBy",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Warehouses_DestinationWarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Warehouses_SourceWarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierProducts_Products_ProductId",
                table: "SupplierProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierProducts_Suppliers_SupplierId",
                table: "SupplierProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Sites_SiteId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_Code",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_SiteId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeCode",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Code",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Warehouses",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Suppliers",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Sites",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "QRValue",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Locations",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Departments",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_SiteId_Code",
                table: "Warehouses",
                columns: new[] { "SiteId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeCode",
                table: "Users",
                column: "EmployeeCode",
                unique: true,
                filter: "\"EmployeeCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Abbreviation",
                table: "Units",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_Name",
                table: "Units",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_QRValue",
                table: "Products",
                column: "QRValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_WarehouseId_Code",
                table: "Locations",
                columns: new[] { "WarehouseId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_UserId",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BarcodeScans_Locations_LocationId",
                table: "BarcodeScans",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BarcodeScans_Products_ProductId",
                table: "BarcodeScans",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BarcodeScans_Stocks_StockId",
                table: "BarcodeScans",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BarcodeScans_Users_ScannedBy",
                table: "BarcodeScans",
                column: "ScannedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Users_InspectedBy",
                table: "Inspections",
                column: "InspectedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickItems_Locations_LocationId",
                table: "PickItems",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickItems_Products_ProductId",
                table: "PickItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickItems_Stocks_StockId",
                table: "PickItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickLists_StockRequests_RequestId",
                table: "PickLists",
                column: "RequestId",
                principalTable: "StockRequests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickLists_Users_AssignedTo",
                table: "PickLists",
                column: "AssignedTo",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PickLists_Warehouses_WarehouseId",
                table: "PickLists",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Products_ProductId",
                table: "PurchaseOrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Sites_SiteId",
                table: "PurchaseOrders",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Users_ApprovedBy",
                table: "PurchaseOrders",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Users_CreatedBy",
                table: "PurchaseOrders",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PutawayItems_Locations_LocationId",
                table: "PutawayItems",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PutawayItems_Products_ProductId",
                table: "PutawayItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PutawayItems_ReceiptItems_ReceiptItemId",
                table: "PutawayItems",
                column: "ReceiptItemId",
                principalTable: "ReceiptItems",
                principalColumn: "ReceiptItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PutawayItems_Stocks_StockId",
                table: "PutawayItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Putaways_Receipts_ReceiptId",
                table: "Putaways",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "ReceiptId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Putaways_Users_CreatedBy",
                table: "Putaways",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Putaways_Warehouses_WarehouseId",
                table: "Putaways",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_Products_ProductId",
                table: "ReceiptItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_PurchaseOrderItems_PurchaseOrderItemId",
                table: "ReceiptItems",
                column: "PurchaseOrderItemId",
                principalTable: "PurchaseOrderItems",
                principalColumn: "PurchaseOrderItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_PurchaseOrders_PurchaseOrderId",
                table: "Receipts",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "PurchaseOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Users_ReceivedBy",
                table: "Receipts",
                column: "ReceivedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Warehouses_WarehouseId",
                table: "Receipts",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_CreatedBy",
                table: "Reports",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_StockRequestItems_RequestItemId",
                table: "Reservations",
                column: "RequestItemId",
                principalTable: "StockRequestItems",
                principalColumn: "RequestItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Stocks_StockId",
                table: "Reservations",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_ReservedBy",
                table: "Reservations",
                column: "ReservedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Products_ProductId",
                table: "StockAdjustments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Stocks_StockId",
                table: "StockAdjustments",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Users_ApprovedBy",
                table: "StockAdjustments",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Users_CreatedBy",
                table: "StockAdjustments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCountItems_Products_ProductId",
                table: "StockCountItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCountItems_Stocks_StockId",
                table: "StockCountItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCounts_Locations_LocationId",
                table: "StockCounts",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCounts_Users_ApprovedBy",
                table: "StockCounts",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCounts_Users_CreatedBy",
                table: "StockCounts",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockCounts_Warehouses_WarehouseId",
                table: "StockCounts",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssueItems_Products_ProductId",
                table: "StockIssueItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssueItems_Stocks_StockId",
                table: "StockIssueItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_Departments_DepartmentId",
                table: "StockIssues",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_PickLists_PickListId",
                table: "StockIssues",
                column: "PickListId",
                principalTable: "PickLists",
                principalColumn: "PickListId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_StockRequests_RequestId",
                table: "StockIssues",
                column: "RequestId",
                principalTable: "StockRequests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_Users_IssuedBy",
                table: "StockIssues",
                column: "IssuedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssues_Warehouses_WarehouseId",
                table: "StockIssues",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequestItems_Products_ProductId",
                table: "StockRequestItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequests_Departments_DepartmentId",
                table: "StockRequests",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequests_Sites_SiteId",
                table: "StockRequests",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequests_Users_ApprovedBy",
                table: "StockRequests",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequests_Users_RequestedBy",
                table: "StockRequests",
                column: "RequestedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturnItems_Products_ProductId",
                table: "StockReturnItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturnItems_Stocks_StockId",
                table: "StockReturnItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturns_Departments_DepartmentId",
                table: "StockReturns",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturns_StockIssues_IssueId",
                table: "StockReturns",
                column: "IssueId",
                principalTable: "StockIssues",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturns_Users_ReturnedBy",
                table: "StockReturns",
                column: "ReturnedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockReturns_Warehouses_WarehouseId",
                table: "StockReturns",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Locations_LocationId",
                table: "Stocks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Products_ProductId",
                table: "Stocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Warehouses_WarehouseId",
                table: "Stocks",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Locations_DestinationLocationId",
                table: "StockTransactions",
                column: "DestinationLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Locations_SourceLocationId",
                table: "StockTransactions",
                column: "SourceLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Products_ProductId",
                table: "StockTransactions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Stocks_StockId",
                table: "StockTransactions",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Users_PerformedBy",
                table: "StockTransactions",
                column: "PerformedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_Locations_DestinationLocationId",
                table: "StockTransferItems",
                column: "DestinationLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_Locations_SourceLocationId",
                table: "StockTransferItems",
                column: "SourceLocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_Products_ProductId",
                table: "StockTransferItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_Stocks_SourceStockId",
                table: "StockTransferItems",
                column: "SourceStockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Users_ApprovedBy",
                table: "StockTransfers",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Users_RequestedBy",
                table: "StockTransfers",
                column: "RequestedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Warehouses_DestinationWarehouseId",
                table: "StockTransfers",
                column: "DestinationWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Warehouses_SourceWarehouseId",
                table: "StockTransfers",
                column: "SourceWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierProducts_Products_ProductId",
                table: "SupplierProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierProducts_Suppliers_SupplierId",
                table: "SupplierProducts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Sites_SiteId",
                table: "Warehouses",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
