using Microsoft.EntityFrameworkCore;
using whm.Models;

namespace whm
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        // =========================================================
        // DbSets
        // =========================================================

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<BarcodeScan> BarcodeScans { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Inspection> Inspections { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<PickItem> PickItems { get; set; }

        public DbSet<PickList> PickLists { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        public DbSet<Putaway> Putaways { get; set; }

        public DbSet<PutawayItem> PutawayItems { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<ReceiptItem> ReceiptItems { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<ReportSchedule> ReportSchedules { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockAdjustment> StockAdjustments { get; set; }

        public DbSet<StockCount> StockCounts { get; set; }

        public DbSet<StockCountItem> StockCountItems { get; set; }

        public DbSet<StockIssue> StockIssues { get; set; }

        public DbSet<StockIssueItem> StockIssueItems { get; set; }

        public DbSet<StockRequest> StockRequests { get; set; }

        public DbSet<StockRequestItem> StockRequestItems { get; set; }

        public DbSet<StockReturn> StockReturns { get; set; }

        public DbSet<StockReturnItem> StockReturnItems { get; set; }

        public DbSet<StockTransaction> StockTransactions { get; set; }

        public DbSet<StockTransfer> StockTransfers { get; set; }

        public DbSet<StockTransferItem> StockTransferItems { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<SupplierProduct> SupplierProducts { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }


        // =========================================================
        // OnModelCreating
        // =========================================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // =====================================================
            // ENUMS -> TEXT
            // =====================================================

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var clrType = property.ClrType;
                    var enumType = Nullable.GetUnderlyingType(clrType) ?? clrType;

                    if (enumType.IsEnum)
                    {
                        var converterType =
                            typeof(Microsoft.EntityFrameworkCore.Storage.ValueConversion.EnumToStringConverter<>)
                            .MakeGenericType(enumType);

                        var converter = Activator.CreateInstance(converterType);

                        property.SetValueConverter((Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter)converter!);

                        property.SetMaxLength(50);
                    }
                }
            }

            // =====================================================
            // PRODUCT
            // =====================================================

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.ProductId);

                entity.Property(x => x.SKU)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Barcode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.QRValue)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(x => x.UnitPrice)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.MinimumStock)
                    .HasColumnType("decimal(18,4)");

                // =============================
                // UNIQUE
                // =============================

                entity.HasIndex(x => x.SKU)
                    .IsUnique();

                entity.HasIndex(x => x.Barcode)
                    .IsUnique();

                entity.HasIndex(x => x.QRValue)
                    .IsUnique();

                // =============================
                // Category
                // =============================

                entity.HasOne(x => x.Category)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                // =============================
                // Unit
                // =============================

                entity.HasOne(x => x.Unit)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // ROLE
            // =====================================================

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(x => x.RoleId);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });


            // =====================================================
            // USER
            // =====================================================

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.UserId);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(x => x.EmployeeCode)
                    .HasMaxLength(50);

                entity.HasIndex(x => x.Email)
                    .IsUnique();

                entity.HasIndex(x => x.EmployeeCode)
                    .IsUnique()
                    .HasFilter("\"EmployeeCode\" IS NOT NULL");

                // Role
                entity.HasOne(x => x.Role)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Department
                entity.HasOne(x => x.Department)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });


            // =====================================================
            // DEPARTMENT
            // =====================================================

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(x => x.DepartmentId);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });


            // =====================================================
            // CATEGORY
            // =====================================================

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.CategoryId);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });


            // =====================================================
            // UNIT
            // =====================================================

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(x => x.UnitId);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(x => x.Name)
                    .IsUnique();

                entity.HasIndex(x => x.Abbreviation)
                    .IsUnique();
            });


            // =====================================================
            // SITE
            // =====================================================

            modelBuilder.Entity<Site>(entity =>
            {
                entity.HasKey(x => x.SiteId);

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(x => x.Code)
                    .IsUnique();
            });


            // =====================================================
            // WAREHOUSE
            // =====================================================

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(x => x.WarehouseId);

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(x => new
                {
                    x.SiteId,
                    x.Code
                }).IsUnique();

                entity.HasOne(x => x.Site)
                    .WithMany(x => x.Warehouses)
                    .HasForeignKey(x => x.SiteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // LOCATION
            // =====================================================

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(x => x.LocationId);

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => new
                {
                    x.WarehouseId,
                    x.Code
                }).IsUnique();

                // Warehouse
                entity.HasOne(x => x.Warehouse)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Parent Location
                entity.HasOne(x => x.ParentLocation)
                    .WithMany(x => x.ChildLocations)
                    .HasForeignKey(x => x.ParentLocationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // SUPPLIER
            // =====================================================

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(x => x.SupplierId);

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(x => x.Email)
                    .HasMaxLength(255);

                entity.Property(x => x.Phone)
                    .HasMaxLength(50);

                entity.HasIndex(x => x.Code)
                    .IsUnique();
            });


            // =====================================================
            // SUPPLIER PRODUCT
            // =====================================================

            modelBuilder.Entity<SupplierProduct>(entity =>
            {
                entity.HasKey(x => x.SupplierProductId);

                entity.Property(x => x.UnitPrice)
                    .HasColumnType("decimal(18,4)");

                entity.HasIndex(x => new
                {
                    x.SupplierId,
                    x.ProductId
                }).IsUnique();

                entity.HasOne(x => x.Supplier)
                    .WithMany(x => x.SupplierProducts)
                    .HasForeignKey(x => x.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.SupplierProducts)
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // PURCHASE ORDER
            // =====================================================

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(x => x.PurchaseOrderId);

                entity.Property(x => x.PONumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.PONumber)
                    .IsUnique();

                entity.Property(x => x.TotalValue)
                    .HasColumnType("decimal(18,4)");

                // Supplier
                entity.HasOne(x => x.Supplier)
                    .WithMany(x => x.PurchaseOrders)
                    .HasForeignKey(x => x.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Site
                entity.HasOne(x => x.Site)
                    .WithMany()
                    .HasForeignKey(x => x.SiteId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Creator
                entity.HasOne(x => x.Creator)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                // Approver
                entity.HasOne(x => x.Approver)
                    .WithMany()
                    .HasForeignKey(x => x.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // PURCHASE ORDER ITEM
            // =====================================================

            modelBuilder.Entity<PurchaseOrderItem>(entity =>
            {
                entity.HasKey(x => x.PurchaseOrderItemId);

                entity.Property(x => x.OrderedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.ReceivedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.RemainingQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.UnitPrice)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.TotalPrice)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.PurchaseOrder)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // RECEIPT
            // =====================================================

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(x => x.ReceiptId);

                entity.Property(x => x.ReceiptNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.ReceiptNumber)
                    .IsUnique();

                entity.HasOne(x => x.PurchaseOrder)
                    .WithMany(x => x.Receipts)
                    .HasForeignKey(x => x.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Receiver)
                    .WithMany()
                    .HasForeignKey(x => x.ReceivedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // RECEIPT ITEM
            // =====================================================

            modelBuilder.Entity<ReceiptItem>(entity =>
            {
                entity.HasKey(x => x.ReceiptItemId);

                entity.Property(x => x.ReceivedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.AcceptedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.QuarantineQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.RejectedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.Receipt)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.ReceiptId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.PurchaseOrderItem)
                    .WithMany(x => x.ReceiptItems)
                    .HasForeignKey(x => x.PurchaseOrderItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // INSPECTION
            // =====================================================

            modelBuilder.Entity<Inspection>(entity =>
            {
                entity.HasKey(x => x.InspectionId);

                entity.HasIndex(x => x.ReceiptItemId)
                    .IsUnique();

                entity.HasOne(x => x.ReceiptItem)
                    .WithOne(x => x.Inspection)
                    .HasForeignKey<Inspection>(x => x.ReceiptItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Inspector)
                    .WithMany()
                    .HasForeignKey(x => x.InspectedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK
            // =====================================================

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(x => x.StockId);

                entity.Property(x => x.StockCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Quantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.ReservedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.AvailableQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.UnitPrice)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.MinimumStock)
                    .HasColumnType("decimal(18,4)");

                entity.HasIndex(x => x.StockCode)
                    .IsUnique();

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Location)
                    .WithMany()
                    .HasForeignKey(x => x.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // PUTAWAY
            // =====================================================

            modelBuilder.Entity<Putaway>(entity =>
            {
                entity.HasKey(x => x.PutawayId);

                entity.Property(x => x.PutawayNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.PutawayNumber)
                    .IsUnique();

                entity.HasOne(x => x.Receipt)
                    .WithMany(x => x.Putaways)
                    .HasForeignKey(x => x.ReceiptId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Creator)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // PUTAWAY ITEM
            // =====================================================

            modelBuilder.Entity<PutawayItem>(entity =>
            {
                entity.HasKey(x => x.PutawayItemId);

                entity.Property(x => x.Quantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.Putaway)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.PutawayId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.ReceiptItem)
                    .WithMany(x => x.PutawayItems)
                    .HasForeignKey(x => x.ReceiptItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Location)
                    .WithMany()
                    .HasForeignKey(x => x.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.PutawayItems)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK REQUEST
            // =====================================================

            modelBuilder.Entity<StockRequest>(entity =>
            {
                entity.HasKey(x => x.RequestId);

                entity.Property(x => x.RequestNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.RequestNumber)
                    .IsUnique();

                entity.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Site)
                    .WithMany()
                    .HasForeignKey(x => x.SiteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Requester)
                    .WithMany()
                    .HasForeignKey(x => x.RequestedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Approver)
                    .WithMany()
                    .HasForeignKey(x => x.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK REQUEST ITEM
            // =====================================================

            modelBuilder.Entity<StockRequestItem>(entity =>
            {
                entity.HasKey(x => x.RequestItemId);

                entity.Property(x => x.RequestedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.ReservedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.IssuedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.RemainingQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.StockRequest)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.RequestId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // RESERVATION
            // =====================================================

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(x => x.ReservationId);

                entity.Property(x => x.Quantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.StockRequest)
                    .WithMany(x => x.Reservations)
                    .HasForeignKey(x => x.RequestId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.RequestItem)
                    .WithMany(x => x.Reservations)
                    .HasForeignKey(x => x.RequestItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.Reservations)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Reserver)
                    .WithMany()
                    .HasForeignKey(x => x.ReservedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // PICK LIST
            // =====================================================

            modelBuilder.Entity<PickList>(entity =>
            {
                entity.HasKey(x => x.PickListId);

                entity.Property(x => x.PickNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.PickNumber)
                    .IsUnique();

                entity.HasOne(x => x.StockRequest)
                    .WithMany(x => x.PickLists)
                    .HasForeignKey(x => x.RequestId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Assignee)
                    .WithMany()
                    .HasForeignKey(x => x.AssignedTo)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // PICK ITEM
            // =====================================================

            modelBuilder.Entity<PickItem>(entity =>
            {
                entity.HasKey(x => x.PickItemId);

                entity.Property(x => x.RequiredQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.PickedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.PickList)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.PickListId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.PickItems)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Location)
                    .WithMany()
                    .HasForeignKey(x => x.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK ISSUE
            // =====================================================

            modelBuilder.Entity<StockIssue>(entity =>
            {
                entity.HasKey(x => x.IssueId);

                entity.Property(x => x.IssueNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.IssueNumber)
                    .IsUnique();

                entity.HasOne(x => x.StockRequest)
                    .WithMany(x => x.StockIssues)
                    .HasForeignKey(x => x.RequestId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.PickList)
                    .WithMany()
                    .HasForeignKey(x => x.PickListId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Issuer)
                    .WithMany()
                    .HasForeignKey(x => x.IssuedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK ISSUE ITEM
            // =====================================================

            modelBuilder.Entity<StockIssueItem>(entity =>
            {
                entity.HasKey(x => x.IssueItemId);

                entity.Property(x => x.Quantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.StockIssue)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.IssueId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.StockIssueItems)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK RETURN
            // =====================================================

            modelBuilder.Entity<StockReturn>(entity =>
            {
                entity.HasKey(x => x.ReturnId);

                entity.Property(x => x.ReturnNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.ReturnNumber)
                    .IsUnique();

                entity.HasOne(x => x.StockIssue)
                    .WithMany()
                    .HasForeignKey(x => x.IssueId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Returner)
                    .WithMany()
                    .HasForeignKey(x => x.ReturnedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK RETURN ITEM
            // =====================================================

            modelBuilder.Entity<StockReturnItem>(entity =>
            {
                entity.HasKey(x => x.ReturnItemId);

                entity.Property(x => x.Quantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.StockReturn)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.ReturnId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.StockReturnItems)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK TRANSFER
            // =====================================================

            modelBuilder.Entity<StockTransfer>(entity =>
            {
                entity.HasKey(x => x.TransferId);

                entity.Property(x => x.TransferNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.TransferNumber)
                    .IsUnique();

                entity.HasOne(x => x.SourceWarehouse)
                    .WithMany()
                    .HasForeignKey(x => x.SourceWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.DestinationWarehouse)
                    .WithMany()
                    .HasForeignKey(x => x.DestinationWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Requester)
                    .WithMany()
                    .HasForeignKey(x => x.RequestedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Approver)
                    .WithMany()
                    .HasForeignKey(x => x.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK TRANSFER ITEM
            // =====================================================

            modelBuilder.Entity<StockTransferItem>(entity =>
            {
                entity.HasKey(x => x.TransferItemId);

                entity.Property(x => x.Quantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.ReceivedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.StockTransfer)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.TransferId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.SourceStock)
                    .WithMany(x => x.StockTransferItems)
                    .HasForeignKey(x => x.SourceStockId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.SourceLocation)
                    .WithMany()
                    .HasForeignKey(x => x.SourceLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.DestinationLocation)
                    .WithMany()
                    .HasForeignKey(x => x.DestinationLocationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK COUNT
            // =====================================================

            modelBuilder.Entity<StockCount>(entity =>
            {
                entity.HasKey(x => x.StockCountId);

                entity.Property(x => x.CountNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.CountNumber)
                    .IsUnique();

                entity.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Location)
                    .WithMany()
                    .HasForeignKey(x => x.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Creator)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Approver)
                    .WithMany()
                    .HasForeignKey(x => x.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK COUNT ITEM
            // =====================================================

            modelBuilder.Entity<StockCountItem>(entity =>
            {
                entity.HasKey(x => x.StockCountItemId);

                entity.Property(x => x.ExpectedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.CountedQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.Variance)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.StockCount)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.StockCountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.StockCountItems)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK ADJUSTMENT
            // =====================================================

            modelBuilder.Entity<StockAdjustment>(entity =>
            {
                entity.HasKey(x => x.AdjustmentId);

                entity.Property(x => x.AdjustmentNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.AdjustmentNumber)
                    .IsUnique();

                entity.Property(x => x.PreviousQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.AdjustmentQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(x => x.NewQuantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.StockAdjustments)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Creator)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Approver)
                    .WithMany()
                    .HasForeignKey(x => x.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // STOCK TRANSACTION
            // =====================================================

            modelBuilder.Entity<StockTransaction>(entity =>
            {
                entity.HasKey(x => x.TransactionId);

                entity.Property(x => x.TransactionType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Quantity)
                    .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.StockTransactions)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.SourceLocation)
                    .WithMany()
                    .HasForeignKey(x => x.SourceLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.DestinationLocation)
                    .WithMany()
                    .HasForeignKey(x => x.DestinationLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Performer)
                    .WithMany()
                    .HasForeignKey(x => x.PerformedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // BARCODE SCAN
            // =====================================================

            modelBuilder.Entity<BarcodeScan>(entity =>
            {
                entity.HasKey(x => x.ScanId);

                entity.Property(x => x.Barcode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.ScanType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.ReferenceType)
                    .HasMaxLength(50);

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.BarcodeScans)
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Stock)
                    .WithMany(x => x.BarcodeScans)
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Location)
                    .WithMany()
                    .HasForeignKey(x => x.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Scanner)
                    .WithMany()
                    .HasForeignKey(x => x.ScannedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // REPORT
            // =====================================================

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(x => x.ReportId);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(x => x.Type)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(x => x.Creator)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // REPORT SCHEDULE
            // =====================================================

            modelBuilder.Entity<ReportSchedule>(entity =>
            {
                entity.HasKey(x => x.ScheduleId);

                entity.Property(x => x.Frequency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(x => x.Report)
                    .WithMany(x => x.Schedules)
                    .HasForeignKey(x => x.ReportId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =====================================================
            // AUDIT LOG
            // =====================================================

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(x => x.AuditLogId);

                entity.Property(x => x.EntityType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Action)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}