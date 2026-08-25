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

        [Required]
        public Guid ReceivedBy { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }

        
        public string? Notes { get; set; }
        [Required]
        public ReceiptStatus receiptStatus { get; set; }=ReceiptStatus.Pending;

        // =========================
        // Navigation Properties
        // =========================

        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public Warehouse Warehouse { get; set; } = null!;

        public User Receiver { get; set; } = null!;

        public List<ReceiptItem> Items { get; set; }
            = new List<ReceiptItem>();

        public List<Putaway> Putaways { get; set; }
            = new List<Putaway>();
    }
}