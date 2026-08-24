using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    //public enum ProductStatus
    //{
    //    Available,
    //    Reserved,
    //    Damage,
    //    Expired,
    //    Quarantined
    //}

    public class Product
    {
        [Key]
        public int ProductId { get; set; }


        // =========================
        // Product Information
        // =========================

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SKU { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Barcode { get; set; }

        [Required]
        [MaxLength(200)]
        public string QRValue { get; set; } = string.Empty;


        // =========================
        // Category
        // =========================

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Categories Category { get; set; } = null!;


        // =========================
        // SubCategory
        // =========================

        [Required]
        public int SubCategoryId { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public SubCategory SubCategory { get; set; } = null!;


        // =========================
        // Status
        // =========================

        //public ProductStatus Status { get; set; }
        //    = ProductStatus.Available;


        // =========================
        // Dates
        // =========================

        public DateTimeOffset CreatedAt { get; set; }
            = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAt { get; set; }


        // =========================
        // Stock
        // =========================

        public List<Stock> Stock { get; set; }
            = new List<Stock>();


        // =========================
        // Operations
        // =========================

        public List<Operations> Operations { get; set; }
            = new List<Operations>();


        // =========================
        // Reports
        // =========================

        public List<Report> reports { get; set; }
            = new List<Report>();


        // =========================
        // Report Schedules
        // =========================

        public List<ReportSchedule> reportSchedules { get; set; }
            = new List<ReportSchedule>();


        // =========================
        // Aliases
        // =========================

        public List<Alias> Aliases { get; set; }
            = new List<Alias>();
        // =========================
        // Product Items
        // =========================

        public List<ProductItem> ProductItems { get; set; }
            = new List<ProductItem>();
    }
}