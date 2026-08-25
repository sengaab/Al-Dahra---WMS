using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum PurchaseOrderStatus
    {
        Draft,
        PendingApproval,
        Approved,
        Rejected,
        Ordered,
        PartiallyReceived,
        Received,
        Cancelled,
        Closed
    }

    public class PurchaseOrder
    {
        [Key]
        public int PurchaseOrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PONumber { get; set; } = string.Empty;

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int SiteId { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public DateTimeOffset? ExpectedDate { get; set; }

        [Required]
        [MaxLength(50)]
        public PurchaseOrderStatus purchaseOrderStatus { get; set; }=PurchaseOrderStatus.Draft;

        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalValue { get; set; } = 0;

        [Required]
        public Guid CreatedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTimeOffset? ApprovedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Supplier Supplier { get; set; } = null!;

        public Site Site { get; set; } = null!;

        public User Creator { get; set; } = null!;

        public User? Approver { get; set; }

        public List<PurchaseOrderItem> Items { get; set; }
            = new List<PurchaseOrderItem>();

        public List<Receipt> Receipts { get; set; }
            = new List<Receipt>();
    }
}