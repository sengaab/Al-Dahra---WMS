using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class StockTransferItem
    {
        [Key]
        public int TransferItemId { get; set; }

        [Required]
        public int TransferId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SourceStockId { get; set; }

        public int? SourceLocationId { get; set; }

        public int? DestinationLocationId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ReceivedQuantity { get; set; } = 0;


        // =========================
        // Navigation Properties
        // =========================

        public StockTransfer StockTransfer { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public Stock SourceStock { get; set; } = null!;

        public Location? SourceLocation { get; set; }

        public Location? DestinationLocation { get; set; }
    }
}