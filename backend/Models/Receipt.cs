using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum ReceiptStatus
    {
        Pending,
        InProgress,
        PendingInspection,
        PartiallyReceived,
        Completed,
        Cancelled
    }

    public class Receipt
    {
        [Key]
        public int ReceiptId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ReceiptNumber { get; set; } = string.Empty;

        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        // Nullable because the receiver is assigned when receiving starts
        public Guid? ReceivedBy { get; set; }

        // Nullable because the receipt does not have a receiving date when first created
        public DateTimeOffset? ReceivedAt { get; set; }

        public string? Notes { get; set; }

        [Required]
        public ReceiptStatus receiptStatus { get; set; } = ReceiptStatus.Pending;

        // =========================
        // Navigation Properties
        // =========================

        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public Warehouse Warehouse { get; set; } = null!;

        public User? Receiver { get; set; }

        public List<ReceiptItem> Items { get; set; }
            = new List<ReceiptItem>();

        public List<Putaway> Putaways { get; set; }
            = new List<Putaway>();
    }
}