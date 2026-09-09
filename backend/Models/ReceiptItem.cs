using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class ReceiptItem
    {
        // =========================
        // Primary Key
        // =========================

        [Key]
        public int ReceiptItemId { get; set; }


        // =========================
        // Required Foreign Keys
        // =========================

        [Required]
        public int ReceiptId { get; set; }

        [Required]
        public int PurchaseOrderItemId { get; set; }

        [Required]
        public int ProductId { get; set; }


        // =========================
        // Quantities
        // =========================

        [Column(TypeName = "decimal(18,4)")]
        public decimal? ReceivedQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? AcceptedQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? QuarantineQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? RejectedQuantity { get; set; }


        // =========================
        // Optional Data
        // =========================

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


        // =========================
        // Collections
        // =========================

        public List<PutawayItem> PutawayItems { get; set; }
            = new List<PutawayItem>();
    }
}