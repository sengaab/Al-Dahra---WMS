using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class StockReturnItem
    {
        [Key]
        public int ReturnItemId { get; set; }

        [Required]
        public int ReturnId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int StockId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string Condition { get; set; } = string.Empty;


        // =========================
        // Navigation Properties
        // =========================

        public StockReturn StockReturn { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public Stock Stock { get; set; } = null!;
    }
}