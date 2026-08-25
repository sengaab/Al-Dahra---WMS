using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum StockStatus
    {
        Available,
        Quarantined,
        Damaged,
        Expired,
        Blocked
    }
    public class Stock
    {
        [Key]
        public int StockId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        public int? LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string StockCode { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal ReservedQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal AvailableQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitPrice { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal MinimumStock { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public StockStatus stockStatus { get; set; } = StockStatus.Available;
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Product Product { get; set; } = null!;

        public Warehouse Warehouse { get; set; } = null!;

        public Location? Location { get; set; }

        public List<PutawayItem> PutawayItems { get; set; }
            = new List<PutawayItem>();

        public List<Reservation> Reservations { get; set; }
            = new List<Reservation>();

        public List<PickItem> PickItems { get; set; }
            = new List<PickItem>();

        public List<StockIssueItem> StockIssueItems { get; set; }
            = new List<StockIssueItem>();

        public List<StockTransferItem> StockTransferItems { get; set; }
            = new List<StockTransferItem>();

        public List<StockReturnItem> StockReturnItems { get; set; }
            = new List<StockReturnItem>();

        public List<StockCountItem> StockCountItems { get; set; }
            = new List<StockCountItem>();

        public List<StockAdjustment> StockAdjustments { get; set; }
            = new List<StockAdjustment>();

        public List<StockTransaction> StockTransactions { get; set; }
            = new List<StockTransaction>();

        public List<BarcodeScan> BarcodeScans { get; set; }
            = new List<BarcodeScan>();
    }
}