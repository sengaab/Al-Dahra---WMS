using Microsoft.EntityFrameworkCore;

namespace whm.Models
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

        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Alias> Aliases { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Site> Sites { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Bin> Bins { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Operations> Operations { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportSchedule> ReportSchedules { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // =====================================================
            // ENUMS → TEXT
            // =====================================================

            //modelBuilder.Entity<Product>()
            //    .Property(p => p.Status)
            //    .HasConversion<string>();

            modelBuilder.Entity<Stock>()
                .Property(s => s.StockStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Stock>()
                .Property(s => s.DeliveryStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Operations>()
                .Property(o => o.OperationType)
                .HasConversion<string>();

            modelBuilder.Entity<Report>()
                .Property(r => r.ReportType)
                .HasConversion<string>();

            modelBuilder.Entity<ReportSchedule>()
                .Property(r => r.ReportType)
                .HasConversion<string>();

            modelBuilder.Entity<ReportSchedule>()
                .Property(r => r.Frequency)
                .HasConversion<string>();

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.Priority)
                .HasConversion<string>();
            /// =====================================================
            // PRODUCT → PRODUCT ITEM
            // One Product → Many ProductItems
            // =====================================================

            modelBuilder.Entity<ProductItem>()
                .HasOne(i => i.Product)
                .WithMany(p => p.ProductItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // STOCK → PRODUCT ITEM
            // One Stock → Many ProductItems
            // =====================================================

            modelBuilder.Entity<ProductItem>()
                .HasOne(i => i.Stock)
                .WithMany(s => s.ProductItems)
                .HasForeignKey(i => i.StockId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // PRODUCT ITEM → UNIQUE ITEM CODE
            // =====================================================

            modelBuilder.Entity<ProductItem>()
                .HasIndex(i => i.ItemCode)
                .IsUnique();


            // =====================================================
            // PRODUCT ITEM → UNIQUE BARCODE
            // =====================================================

            modelBuilder.Entity<ProductItem>()
                .HasIndex(i => i.Barcode)
                .IsUnique();


            // =====================================================
            // PRODUCT ITEM → UNIQUE QR
            // =====================================================

            modelBuilder.Entity<ProductItem>()
                .HasIndex(i => i.QRValue)
                .IsUnique();

            // =====================================================
            // ROLE
            // =====================================================

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Role_Name)
                .IsUnique();


            // =====================================================
            // ROLE SEED DATA
            // =====================================================

            modelBuilder.Entity<Role>().HasData(

                new Role
                {
                    Role_Id = 1,
                    Role_Name = "Admin",
                    Role_Description = "System Administrator",
                    IsActive = true,
                    CreateAt = new DateTimeOffset(
                        new DateTime(2026, 1, 1),
                        TimeSpan.Zero),
                    UpdateAt = new DateTimeOffset(
                        new DateTime(2026, 1, 1),
                        TimeSpan.Zero)
                },

                new Role
                {
                    Role_Id = 2,
                    Role_Name = "Employee",
                    Role_Description = "Normal Employee",
                    IsActive = true,
                    CreateAt = new DateTimeOffset(
                        new DateTime(2026, 1, 1),
                        TimeSpan.Zero),
                    UpdateAt = new DateTimeOffset(
                        new DateTime(2026, 1, 1),
                        TimeSpan.Zero)
                },

                new Role
                {
                    Role_Id = 3,
                    Role_Name = "Manager",
                    Role_Description = "Warehouse Manager",
                    IsActive = true,
                    CreateAt = new DateTimeOffset(
                        new DateTime(2026, 1, 1),
                        TimeSpan.Zero),
                    UpdateAt = new DateTimeOffset(
                        new DateTime(2026, 1, 1),
                        TimeSpan.Zero)
                }
            );


            // =====================================================
            // DECIMAL PRECISION
            // =====================================================

            modelBuilder.Entity<Stock>()
                .Property(s => s.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Stock>()
                .Property(s => s.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Operations>()
                .Property(o => o.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.TaxRate)
                .HasPrecision(5, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.ReceivedQuantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Order>()
                .Property(o => o.Subtotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TaxAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);


            // =====================================================
            // USERS → ROLE
            // One Role → Many Users
            // =====================================================

            modelBuilder.Entity<Users>()
                .HasOne(u => u.role)
                .WithMany(r => r.User)
                .HasForeignKey(u => u.Role_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // USER EMAIL UNIQUE
            // =====================================================

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.User_Email)
                .IsUnique();


            // =====================================================
            // DEPARTMENT → CATEGORY
            // One Department → Many Categories
            // =====================================================

            modelBuilder.Entity<Categories>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Categories)
                .HasForeignKey(c => c.Department_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // CATEGORY → PRODUCT
            // One Category → Many Products
            // =====================================================

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // CATEGORY → SUBCATEGORY
            // One Category → Many SubCategories
            // =====================================================

            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // SUBCATEGORY → PRODUCT
            // One SubCategory → Many Products
            // =====================================================

            modelBuilder.Entity<Product>()
                .HasOne(p => p.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // UNIT → PRODUCT
            // One Unit → Many Products
            // =====================================================

            // ملاحظة:
            // الـ Product الحالي اللي بعتّه لا يحتوي UnitId.
            // لذلك لا نضيف علاقة Product → Unit هنا.


            // =====================================================
            // SITE → WAREHOUSE
            // One Site → Many Warehouses
            // =====================================================

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Site)
                .WithMany(s => s.Warehouses)
                .HasForeignKey(w => w.Site_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // WAREHOUSE → ROOM
            // One Warehouse → Many Rooms
            // =====================================================

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Warehouse)
                .WithMany(w => w.Rooms)
                .HasForeignKey(r => r.Warehouse_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // ROOM → ROW
            // One Room → Many Rows
            // =====================================================

            modelBuilder.Entity<Row>()
                .HasOne(r => r.Room)
                .WithMany(ro => ro.Rows)
                .HasForeignKey(r => r.Room_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // ROW → SHELF
            // One Row → Many Shelves
            // =====================================================

            modelBuilder.Entity<Shelf>()
                .HasOne(s => s.Row)
                .WithMany(r => r.Shelves)
                .HasForeignKey(s => s.Row_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // SHELF → BIN
            // One Shelf → Many Bins
            // =====================================================

            modelBuilder.Entity<Bin>()
                .HasOne(b => b.Shelf)
                .WithMany(s => s.Bins)
                .HasForeignKey(b => b.Shelf_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // PRODUCT → STOCK
            // One Product → Many Stocks
            // =====================================================

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stock)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // BIN → STOCK
            // One Bin → Many Stocks
            // =====================================================

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Bin)
                .WithMany(b => b.Stocks)
                .HasForeignKey(s => s.Bin_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // UNIT → STOCK
            // One Unit → Many Stocks
            // =====================================================

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Units)
                .WithMany(u => u.Stocks)
                .HasForeignKey(s => s.UnitId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // PRODUCT → ALIAS
            // One Product → Many Aliases
            // =====================================================

            modelBuilder.Entity<Alias>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Aliases)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // OPERATIONS → PRODUCT
            // One Product → Many Operations
            // =====================================================

            modelBuilder.Entity<Operations>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Operations)
                .HasForeignKey(o => o.Product_Id)
                .OnDelete(DeleteBehavior.Restrict);




            // =====================================================
            // UNIQUE SKU

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();
        


        // =====================================================
        // OPERATIONS → UNIT
        // One Unit → Many Operations
        // =====================================================

        modelBuilder.Entity<Operations>()
                .HasOne(o => o.Unit)
                .WithMany(u => u.operations)
                .HasForeignKey(o => o.Unit_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // OPERATIONS → USER
            // One User → Many Operations
            // =====================================================

            modelBuilder.Entity<Operations>()
                .HasOne(o => o.User)
                .WithMany(u => u.Operations)
                .HasForeignKey(o => o.User_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // OPERATIONS → FROM BIN
            // One Bin → Many Operations
            // =====================================================

            modelBuilder.Entity<Operations>()
                .HasOne(o => o.FromBin)
                .WithMany(b => b.FromOperation)
                .HasForeignKey(o => o.FromBinId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // OPERATIONS → TO BIN
            // One Bin → Many Operations
            // =====================================================

            modelBuilder.Entity<Operations>()
                .HasOne(o => o.ToBin)
                .WithMany(b => b.ToOperation)
                .HasForeignKey(o => o.ToBinId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // SUPPLIER → ORDER
            // One Supplier → Many Orders
            // =====================================================

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // WAREHOUSE → ORDER
            // One Warehouse → Many Orders
            // =====================================================

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Warehouse)
                .WithMany()
                .HasForeignKey(o => o.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // ORDER → CREATED BY USER
            // One User → Many Orders
            // =====================================================

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CreatedByUser)
                .WithMany()
                .HasForeignKey(o => o.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // ORDER → APPROVED BY USER
            // One User → Many Approved Orders
            // =====================================================

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ApprovedByUser)
                .WithMany()
                .HasForeignKey(o => o.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // ORDER → ORDER ITEMS
            // One Order → Many OrderItems
            // =====================================================

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // PRODUCT → ORDER ITEMS
            // One Product → Many OrderItems
            // =====================================================

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // REPORT → USER
            // One User → Many Reports
            // =====================================================

            modelBuilder.Entity<Report>()
                .HasOne(r => r.CreateByUser)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.craeteByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // REPORT → WAREHOUSE
            // One Warehouse → Many Reports
            // =====================================================

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Warehouses)
                .WithMany(w => w.reports)
                .HasForeignKey(r => r.Warehouse_Id)
                .OnDelete(DeleteBehavior.Restrict);
            //Unique SKU



            // =====================================================
            // REPORT → PRODUCT
            // One Product → Many Reports
            // =====================================================

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Products)
                .WithMany(p => p.reports)
                .HasForeignKey(r => r.Product_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // REPORT SCHEDULE → USER
            // One User → Many ReportSchedules
            // =====================================================

            modelBuilder.Entity<ReportSchedule>()
                .HasOne(r => r.CreateByUser)
                .WithMany(u => u.reportSchedules)
                .HasForeignKey(r => r.craeteByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // REPORT SCHEDULE → WAREHOUSE
            // One Warehouse → Many ReportSchedules
            // =====================================================

            modelBuilder.Entity<ReportSchedule>()
                .HasOne(r => r.Warehouses)
                .WithMany(w => w.reportSchedules)
                .HasForeignKey(r => r.Warehouse_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // REPORT SCHEDULE → PRODUCT
            // One Product → Many ReportSchedules
            // =====================================================

            modelBuilder.Entity<ReportSchedule>()
                .HasOne(r => r.Products)
                .WithMany(p => p.reportSchedules)
                .HasForeignKey(r => r.Product_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // AUDIT LOG → USER
            // One User → Many AuditLogs
            // =====================================================

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Users)
                .WithMany(u => u.auditLog)
                .HasForeignKey(a => a.User_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // UNIQUE INDEXES
            // =====================================================

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.QRValue)
                .IsUnique();

            modelBuilder.Entity<Alias>()
                .HasIndex(a => new
                {
                    a.ProductId,
                    a.AliasName
                })
                .IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasIndex(s => s.SupplierCode)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Stock>()
                .HasIndex(s => s.StockCode)
                .IsUnique();

        }
    }
}