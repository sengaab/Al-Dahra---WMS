using Microsoft.EntityFrameworkCore;
using whm.Models;

namespace whm.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        // =====================================================
        // DbSets
        // =====================================================

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Site> Sites { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }
        public DbSet<Inspection> Inspections { get; set; }

        public DbSet<Putaway> Putaways { get; set; }
        public DbSet<PutawayItem> PutawayItems { get; set; }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }

        public DbSet<StockRequest> StockRequests { get; set; }
        public DbSet<StockRequestItem> StockRequestItems { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<PickList> PickLists { get; set; }
        public DbSet<PickItem> PickItems { get; set; }

        public DbSet<StockIssue> StockIssues { get; set; }
        public DbSet<StockIssueItem> StockIssueItems { get; set; }

        public DbSet<StockReturn> StockReturns { get; set; }
        public DbSet<StockReturnItem> StockReturnItems { get; set; }

        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<StockTransferItem> StockTransferItems { get; set; }

        public DbSet<StockCount> StockCounts { get; set; }
        public DbSet<StockCountItem> StockCountItems { get; set; }

        public DbSet<BarcodeScan> BarcodeScans { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportSchedule> ReportSchedules { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================================
            // ENUMS -> TEXT
            // =====================================================

            modelBuilder.Entity<Inspection>()
                .Property(x => x.InspectionStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<PickItem>()
                .Property(x => x.pickItemStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<PickList>()
                .Property(x => x.PickListStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Product>()
                .Property(x => x.ProductStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(x => x.purchaseOrderStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Putaway>()
                .Property(x => x.StatusStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Receipt>()
                .Property(x => x.receiptStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Reservation>()
                .Property(x => x.reservationStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Stock>()
                .Property(x => x.stockStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<StockAdjustment>()
                .Property(x => x.StockAdjustmentStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<StockCount>()
                .Property(x => x.stockCountStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<StockIssue>()
                .Property(x => x.StockIssueStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<StockRequest>()
                .Property(x => x.StockRequestStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<StockReturn>()
                .Property(x => x.stockReturnStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<StockTransfer>()
                .Property(x => x.TransferStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Supplier>()
                .Property(x => x.SupplierStatus)
                .HasConversion<string>()
                .HasMaxLength(50);


            // =====================================================
            // DECIMAL PRECISION
            // =====================================================

            modelBuilder.Entity<Product>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Product>()
                .Property(x => x.MinimumStock)
                .HasPrecision(18, 4);

            modelBuilder.Entity<SupplierProduct>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(x => x.TotalValue)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.OrderedQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.ReceivedQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.RemainingQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.TotalPrice)
                .HasPrecision(18, 4);


            // =====================================================
            // UNIQUE INDEXES
            // =====================================================

            modelBuilder.Entity<Product>()
                .HasIndex(x => x.SKU)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(x => x.Barcode)
                .IsUnique();

            modelBuilder.Entity<Warehouse>()
                .HasIndex(x => x.Code)
                .IsUnique();

            modelBuilder.Entity<Site>()
                .HasIndex(x => x.Code)
                .IsUnique();

            modelBuilder.Entity<Location>()
                .HasIndex(x => x.Code)
                .IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasIndex(x => x.Code)
                .IsUnique();

            modelBuilder.Entity<PurchaseOrder>()
                .HasIndex(x => x.PONumber)
                .IsUnique();

            modelBuilder.Entity<Receipt>()
                .HasIndex(x => x.ReceiptNumber)
                .IsUnique();

            modelBuilder.Entity<Putaway>()
                .HasIndex(x => x.PutawayNumber)
                .IsUnique();

            modelBuilder.Entity<Stock>()
                .HasIndex(x => x.StockCode)
                .IsUnique();

            modelBuilder.Entity<StockRequest>()
                .HasIndex(x => x.RequestNumber)
                .IsUnique();

            modelBuilder.Entity<PickList>()
                .HasIndex(x => x.PickNumber)
                .IsUnique();

            modelBuilder.Entity<StockIssue>()
                .HasIndex(x => x.IssueNumber)
                .IsUnique();

            modelBuilder.Entity<StockReturn>()
                .HasIndex(x => x.ReturnNumber)
                .IsUnique();

            modelBuilder.Entity<StockTransfer>()
                .HasIndex(x => x.TransferNumber)
                .IsUnique();

            modelBuilder.Entity<StockCount>()
                .HasIndex(x => x.CountNumber)
                .IsUnique();

            modelBuilder.Entity<StockAdjustment>()
                .HasIndex(x => x.AdjustmentNumber)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.EmployeeCode)
                .IsUnique();


            // =====================================================
            // ROLE -> USERS
            // =====================================================

            modelBuilder.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // DEPARTMENT -> USERS
            // =====================================================

            modelBuilder.Entity<User>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // SITE -> WAREHOUSES
            // =====================================================

            modelBuilder.Entity<Warehouse>()
                .HasOne(x => x.Site)
                .WithMany(x => x.Warehouses)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // WAREHOUSE -> LOCATIONS
            // =====================================================

            modelBuilder.Entity<Location>()
                .HasOne(x => x.Warehouse)
                .WithMany(x => x.Locations)
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // LOCATION -> CHILD LOCATIONS
            // =====================================================

            modelBuilder.Entity<Location>()
                .HasOne(x => x.ParentLocation)
                .WithMany(x => x.ChildLocations)
                .HasForeignKey(x => x.ParentLocationId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // CATEGORY -> PRODUCTS
            // =====================================================

            modelBuilder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // UNIT -> PRODUCTS
            // =====================================================

            modelBuilder.Entity<Product>()
                .HasOne(x => x.Unit)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // SUPPLIER <-> PRODUCT
            // =====================================================

            modelBuilder.Entity<SupplierProduct>()
                .HasOne(x => x.Supplier)
                .WithMany(x => x.SupplierProducts)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierProduct>()
                .HasOne(x => x.Product)
                .WithMany(x => x.SupplierProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierProduct>()
                .HasIndex(x => new { x.SupplierId, x.ProductId })
                .IsUnique();


            // =====================================================
            // PURCHASE ORDER
            // =====================================================

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(x => x.Supplier)
                .WithMany(x => x.PurchaseOrders)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(x => x.Site)
                .WithMany()
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // PURCHASE ORDER -> ITEMS
            // =====================================================

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(x => x.PurchaseOrder)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // RECEIPT
            // =====================================================

            modelBuilder.Entity<Receipt>()
                .HasOne(x => x.PurchaseOrder)
                .WithMany(x => x.Receipts)
                .HasForeignKey(x => x.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Receipt>()
                .HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Receipt>()
                .HasOne(x => x.Receiver)
                .WithMany()
                .HasForeignKey(x => x.ReceivedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // RECEIPT ITEMS
            // =====================================================

            modelBuilder.Entity<ReceiptItem>()
                .HasOne(x => x.Receipt)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReceiptItem>()
                .HasOne(x => x.PurchaseOrderItem)
                .WithMany(x => x.ReceiptItems)
                .HasForeignKey(x => x.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReceiptItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // INSPECTION
            // =====================================================

            modelBuilder.Entity<Inspection>()
                .HasOne(x => x.ReceiptItem)
                .WithOne(x => x.Inspection)
                .HasForeignKey<Inspection>(x => x.ReceiptItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inspection>()
                .HasOne(x => x.Inspector)
                .WithMany()
                .HasForeignKey(x => x.InspectedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // PUTAWAY
            // =====================================================

            modelBuilder.Entity<Putaway>()
                .HasOne(x => x.Receipt)
                .WithMany(x => x.Putaways)
                .HasForeignKey(x => x.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Putaway>()
                .HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Putaway>()
                .HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // PUTAWAY ITEMS
            // =====================================================

            modelBuilder.Entity<PutawayItem>()
                .HasOne(x => x.Putaway)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.PutawayId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PutawayItem>()
                .HasOne(x => x.ReceiptItem)
                .WithMany(x => x.PutawayItems)
                .HasForeignKey(x => x.ReceiptItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PutawayItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PutawayItem>()
                .HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PutawayItem>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.PutawayItems)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK
            // =====================================================

            modelBuilder.Entity<Stock>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stock>()
                .HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stock>()
                .HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // RESERVATION
            // =====================================================

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.StockRequest)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.RequestItem)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.RequestItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.Reserver)
                .WithMany()
                .HasForeignKey(x => x.ReservedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK REQUEST
            // =====================================================

            modelBuilder.Entity<StockRequest>()
                .HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockRequest>()
                .HasOne(x => x.Site)
                .WithMany()
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockRequest>()
                .HasOne(x => x.Requester)
                .WithMany()
                .HasForeignKey(x => x.RequestedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockRequest>()
                .HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK REQUEST ITEMS
            // =====================================================

            modelBuilder.Entity<StockRequestItem>()
                .HasOne(x => x.StockRequest)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockRequestItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // PICK LIST
            // =====================================================

            modelBuilder.Entity<PickList>()
                .HasOne(x => x.StockRequest)
                .WithMany(x => x.PickLists)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PickList>()
                .HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PickList>()
                .HasOne(x => x.Assignee)
                .WithMany()
                .HasForeignKey(x => x.AssignedTo)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // PICK ITEMS
            // =====================================================

            modelBuilder.Entity<PickItem>()
                .HasOne(x => x.PickList)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.PickListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PickItem>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.PickItems)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PickItem>()
                .HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PickItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK ISSUE
            // =====================================================

            modelBuilder.Entity<StockIssue>()
                .HasOne(x => x.StockRequest)
                .WithMany(x => x.StockIssues)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockIssue>()
                .HasOne(x => x.PickList)
                .WithMany()
                .HasForeignKey(x => x.PickListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockIssue>()
                .HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockIssue>()
                .HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockIssue>()
                .HasOne(x => x.Issuer)
                .WithMany()
                .HasForeignKey(x => x.IssuedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK ISSUE ITEMS
            // =====================================================

            modelBuilder.Entity<StockIssueItem>()
                .HasOne(x => x.StockIssue)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockIssueItem>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.StockIssueItems)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockIssueItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK RETURN
            // =====================================================

            modelBuilder.Entity<StockReturn>()
                .HasOne(x => x.StockIssue)
                .WithMany()
                .HasForeignKey(x => x.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockReturn>()
                .HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockReturn>()
                .HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockReturn>()
                .HasOne(x => x.Returner)
                .WithMany()
                .HasForeignKey(x => x.ReturnedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK RETURN ITEMS
            // =====================================================

            modelBuilder.Entity<StockReturnItem>()
                .HasOne(x => x.StockReturn)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ReturnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockReturnItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockReturnItem>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.StockReturnItems)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK TRANSFER
            // =====================================================

            modelBuilder.Entity<StockTransfer>()
                .HasOne(x => x.SourceWarehouse)
                .WithMany()
                .HasForeignKey(x => x.SourceWarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(x => x.DestinationWarehouse)
                .WithMany()
                .HasForeignKey(x => x.DestinationWarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(x => x.Requester)
                .WithMany()
                .HasForeignKey(x => x.RequestedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK TRANSFER ITEMS
            // =====================================================

            modelBuilder.Entity<StockTransferItem>()
                .HasOne(x => x.StockTransfer)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.TransferId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransferItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransferItem>()
                .HasOne(x => x.SourceStock)
                .WithMany(x => x.StockTransferItems)
                .HasForeignKey(x => x.SourceStockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransferItem>()
                .HasOne(x => x.SourceLocation)
                .WithMany()
                .HasForeignKey(x => x.SourceLocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransferItem>()
                .HasOne(x => x.DestinationLocation)
                .WithMany()
                .HasForeignKey(x => x.DestinationLocationId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK COUNT
            // =====================================================

            modelBuilder.Entity<StockCount>()
                .HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockCount>()
                .HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockCount>()
                .HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockCount>()
                .HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK COUNT ITEMS
            // =====================================================

            modelBuilder.Entity<StockCountItem>()
                .HasOne(x => x.StockCount)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.StockCountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockCountItem>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.StockCountItems)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockCountItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK ADJUSTMENT
            // =====================================================

            modelBuilder.Entity<StockAdjustment>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.StockAdjustments)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockAdjustment>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockAdjustment>()
                .HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockAdjustment>()
                .HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // STOCK TRANSACTION
            // =====================================================

            modelBuilder.Entity<StockTransaction>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransaction>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.StockTransactions)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransaction>()
                .HasOne(x => x.SourceLocation)
                .WithMany()
                .HasForeignKey(x => x.SourceLocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransaction>()
                .HasOne(x => x.DestinationLocation)
                .WithMany()
                .HasForeignKey(x => x.DestinationLocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransaction>()
                .HasOne(x => x.Performer)
                .WithMany()
                .HasForeignKey(x => x.PerformedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // BARCODE SCAN
            // =====================================================

            modelBuilder.Entity<BarcodeScan>()
      .HasOne(x => x.Product)
      .WithMany(x => x.BarcodeScans)
      .HasForeignKey(x => x.ProductId)
      .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BarcodeScan>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.BarcodeScans)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BarcodeScan>()
                .HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BarcodeScan>()
                .HasOne(x => x.Scanner)
                .WithMany()
                .HasForeignKey(x => x.ScannedBy)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // REPORT
            // =====================================================

            modelBuilder.Entity<Report>()
                .HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReportSchedule>()
                .HasOne(x => x.Report)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // AUDIT LOG
            // =====================================================

            modelBuilder.Entity<AuditLog>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // DEFAULT VALUES
            // =====================================================

            modelBuilder.Entity<User>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Role>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Department>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Site>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Warehouse>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Location>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Product>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Supplier>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Stock>()
          .HasOne(x => x.Supplier)
          .WithMany(x => x.Stocks)
         .HasForeignKey(x => x.SupplierId)
         .OnDelete(DeleteBehavior.Restrict);
        }
    }
}