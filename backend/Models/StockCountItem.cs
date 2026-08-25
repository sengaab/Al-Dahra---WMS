using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class StockCountItem
    {
        [Key]
        public int StockCountItemId { get; set; }

        [Required]
        public int StockCountId { get; set; }

        [Required]
        public int StockId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ExpectedQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal CountedQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Variance { get; set; } = 0;

        public string? Reason { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public StockCount StockCount { get; set; } = null!;

        public Stock Stock { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}
