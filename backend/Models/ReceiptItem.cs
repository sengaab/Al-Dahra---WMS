using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class ReceiptItem
    {
        [Key]
        public int ReceiptItemId { get; set; }

        [Required]
        public int ReceiptId { get; set; }

        [Required]
        public int PurchaseOrderItemId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ReceivedQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal AcceptedQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal QuarantineQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal RejectedQuantity { get; set; } = 0;

        [MaxLength(100)]
        public string? BatchNumber { get; set; }

        public DateOnly? ExpiryDate { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Receipt Receipt { get; set; } = null!;

        public PurchaseOrderItem PurchaseOrderItem { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public Inspection? Inspection { get; set; }

        public List<PutawayItem> PutawayItems { get; set; }
            = new List<PutawayItem>();
    }
}