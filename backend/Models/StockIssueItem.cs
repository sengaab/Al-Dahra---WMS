using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class StockIssueItem
    {
        [Key]
        public int IssueItemId { get; set; }

        [Required]
        public int IssueId { get; set; }

        [Required]
        public int StockId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public StockIssue StockIssue { get; set; } = null!;

        public Stock Stock { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}
