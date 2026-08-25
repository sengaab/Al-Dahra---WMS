using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class PutawayItem
    {
        [Key]
        public int PutawayItemId { get; set; }

        [Required]
        public int PutawayId { get; set; }

        [Required]
        public int ReceiptItemId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }

        [Required]
        public int StockId { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Putaway Putaway { get; set; } = null!;

        public ReceiptItem ReceiptItem { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public Location Location { get; set; } = null!;

        public Stock Stock { get; set; } = null!;
    }
}