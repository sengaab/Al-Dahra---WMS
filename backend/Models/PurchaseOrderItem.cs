using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
   
    public class PurchaseOrderItem
    {
        [Key]
        public int PurchaseOrderItemId { get; set; }

        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal OrderedQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ReceivedQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal RemainingQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitPrice { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalPrice { get; set; } = 0;
       


        // =========================
        // Navigation Properties
        // =========================

        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public List<ReceiptItem> ReceiptItems { get; set; }
            = new List<ReceiptItem>();
    }
}
