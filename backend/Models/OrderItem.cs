using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum OrderStatus
    {
        Draft,
        Pending,
        Approved,
        Ordered,
        PartiallyReceived,
        Received,
        Cancelled
    }

    public enum OrderPriority
    {
        Low,
        Normal,
        High,
        Urgent
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }


        // =========================
        // Order Number
        // =========================

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;


        // =========================
        // Supplier
        // =========================

        [Required]
        public int SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;


        // =========================
        // Dates
        // =========================

        public DateTimeOffset OrderDate { get; set; }
            = DateTimeOffset.UtcNow;

        public DateTimeOffset? ExpectedDate { get; set; }


        // =========================
        // Status
        // =========================

        public OrderStatus Status { get; set; }
            = OrderStatus.Draft;


        // =========================
        // Priority
        // =========================

        public OrderPriority Priority { get; set; }
            = OrderPriority.Normal;


        // =========================
        // Warehouse
        // =========================

        [Required]
        public int WarehouseId { get; set; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; } = null!;


        // =========================
        // Created By
        // =========================

        [Required]
        public Guid CreatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public Users CreatedByUser { get; set; } = null!;


        // =========================
        // Approved By
        // =========================

        public Guid? ApprovedBy { get; set; }

        [ForeignKey(nameof(ApprovedBy))]
        public Users? ApprovedByUser { get; set; }


        // =========================
        // Notes
        // =========================

        [MaxLength(1000)]
        public string? Notes { get; set; }


        // =========================
        // Amounts
        // =========================

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }


        // =========================
        // Dates
        // =========================

        public DateTimeOffset CreatedAt { get; set; }
            = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAt { get; set; }


        // =========================
        // Order Items
        // =========================

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}