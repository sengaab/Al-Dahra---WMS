using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class StockTransaction
    {
        [Key]
        public long TransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int StockId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }

        public int? SourceLocationId { get; set; }

        public int? DestinationLocationId { get; set; }

        [MaxLength(50)]
        public string? ReferenceType { get; set; }

        public int? ReferenceId { get; set; }

        [Required]
        public Guid PerformedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string? Notes { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Product Product { get; set; } = null!;

        public Stock Stock { get; set; } = null!;

        public Location? SourceLocation { get; set; }

        public Location? DestinationLocation { get; set; }

        public User Performer { get; set; } = null!;
    }
}