using Microsoft.EntityFrameworkCore;

namespace whm.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        // =========================
        // DbSets
        // =========================

        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Bin> Bins { get; set; }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportSchedule> ReportSchedules { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // =====================================================
            // Roles
            // =====================================================

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Role_Name)
                .IsUnique();


            // =====================================================
            // Roles Seed Data
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
            // Decimal Precision
            // =====================================================

            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Stock>()
                .Property(s => s.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Quantity)
                .HasPrecision(18, 3);


            // =====================================================
            // Users → Role
            // One Role → Many Users
            // =====================================================

            modelBuilder.Entity<Users>()
                .HasOne(u => u.role)
                .WithMany(r => r.User)
                .HasForeignKey(u => u.Role_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // User Email Unique
            // =====================================================

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.User_Email)
                .IsUnique();


            // =====================================================
            // Department → Category
            // One Department → Many Categories
            // =====================================================

            modelBuilder.Entity<Categories>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Categories)
                .HasForeignKey(c => c.Department_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Category → Product
            // One Category → Many Products
            // =====================================================

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Unit → Product
            // One Unit → Many Products
            // =====================================================

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Units)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Warehouse → Room
            // =====================================================

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Warehouse)
                .WithMany(w => w.Rooms)
                .HasForeignKey(r => r.Warehouse_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Room → Row
            // =====================================================

            modelBuilder.Entity<Row>()
                .HasOne(r => r.Room)
                .WithMany(ro => ro.Rows)
                .HasForeignKey(r => r.Room_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Row → Shelf
            // =====================================================

            modelBuilder.Entity<Shelf>()
                .HasOne(s => s.Row)
                .WithMany(r => r.Shelves)
                .HasForeignKey(s => s.Row_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Shelf → Bin
            // =====================================================

            modelBuilder.Entity<Bin>()
                .HasOne(b => b.Shelf)
                .WithMany(s => s.Bins)
                .HasForeignKey(b => b.Shelf_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Product → Stock
            // =====================================================

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stock)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Bin → Stock
            // =====================================================

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Bin)
                .WithMany(b => b.Stocks)
                .HasForeignKey(s => s.Bin_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Transaction → Product
            // =====================================================

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Product)
                .WithMany(p => p.transactions)
                .HasForeignKey(t => t.Product_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Transaction → Unit
            // =====================================================

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Unit)
                .WithMany(u => u.transactions)
                .HasForeignKey(t => t.Unit_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Transaction → User
            // =====================================================

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.transactions)
                .HasForeignKey(t => t.User_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Transaction → From Bin
            // =====================================================

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FromBin)
                .WithMany(b => b.Fromtransactions)
                .HasForeignKey(t => t.FromBinId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Transaction → To Bin
            // =====================================================

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToBin)
                .WithMany(b => b.Totransactions)
                .HasForeignKey(t => t.ToBinId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Report → User
            // =====================================================

            modelBuilder.Entity<Report>()
                .HasOne(r => r.CreateByUser)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.craeteByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Report → Warehouse
            // =====================================================

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Warehouses)
                .WithMany(w => w.reports)
                .HasForeignKey(r => r.Warehouse_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // Report → Product
            // =====================================================

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Products)
                .WithMany(p => p.reports)
                .HasForeignKey(r => r.Product_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // ReportSchedule → User
            // =====================================================

            modelBuilder.Entity<ReportSchedule>()
                .HasOne(r => r.CreateByUser)
                .WithMany(u => u.reportSchedules)
                .HasForeignKey(r => r.craeteByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // ReportSchedule → Warehouse
            // =====================================================

            modelBuilder.Entity<ReportSchedule>()
                .HasOne(r => r.Warehouses)
                .WithMany(w => w.reportSchedules)
                .HasForeignKey(r => r.Warehouse_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // ReportSchedule → Product
            // =====================================================

            modelBuilder.Entity<ReportSchedule>()
                .HasOne(r => r.Products)
                .WithMany(p => p.reportSchedules)
                .HasForeignKey(r => r.Product_Id)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // AuditLog → User
            // =====================================================

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Users)
                .WithMany(u => u.auditLog)
                .HasForeignKey(a => a.User_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}