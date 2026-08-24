using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    // =========================
    // Stock Status
    // =========================

    public enum StockStatus
    {
        Available,
        Reserved,
        Expired,
        Quarantined
    }


    // =========================
    // Delivery Status
    // =========================

    public enum DeliveryStatus
    {
        Pending,
        InTransit,
        PartiallyReceived,
        Delivered,
        Cancelled
    }


    public class Stock
    {
        // =========================
        // Primary Key
        // =========================

        [Key]
        public int Stock_Id { get; set; }


        // =========================
        // Stock / Lot Code
        // =========================

        [Required]
        [MaxLength(50)]
        public string StockCode { get; set; } = string.Empty;


        // =========================
        // Quantity
        // =========================

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Quantity { get; set; }


        // =========================
        // Expiry Date
        // =========================

        public DateTime? ExpiryDate { get; set; }

        // =========================
        // Reserved Quantity
        // =========================

        [Range(0, int.MaxValue)]
        public int ReservedQuantity { get; set; } = 0;


        // =========================
        // Unit Price
        // =========================

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }


        // =========================
        // Minimum Stock
        // =========================

        public int MinimumStock { get; set; }


        // =========================
        // Unit
        // =========================

        public int? UnitId { get; set; }

        [ForeignKey(nameof(UnitId))]
        public Unit? Units { get; set; }


        // =========================
        // Product
        // =========================

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;


        // =========================
        // Bin
        // =========================

        public int? Bin_Id { get; set; }

        [ForeignKey(nameof(Bin_Id))]
        public Bin? Bin { get; set; }


        // =========================
        // Stock Status
        // =========================

        public StockStatus StockStatus { get; set; }
            = StockStatus.Available;


        // =========================
        // Delivery Status
        // =========================

        public DeliveryStatus DeliveryStatus { get; set; }
            = DeliveryStatus.Pending;


        // =========================
        // Active
        // =========================

        public bool IsActive { get; set; } = true;


        // =========================
        // Dates
        // =========================

        public DateTime CreateAt { get; set; }
            = DateTime.UtcNow;

        public DateTime LastUpdatedAt { get; set; }
            = DateTime.UtcNow;
        // =========================
        // Product Items
        // =========================

        public List<ProductItem> ProductItems { get; set; }
            = new List<ProductItem>();
    }
}