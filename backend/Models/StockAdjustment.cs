using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum StockAdjustmentStatus
    {
        Pending,
        Approved,
        Rejected,
        Applied,
        Cancelled
    }
    public class StockAdjustment
    {
        [Key]
        public int AdjustmentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string AdjustmentNumber { get; set; } = string.Empty;

        [Required]
        public int StockId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal PreviousQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal AdjustmentQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal NewQuantity { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public Guid CreatedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        [MaxLength(50)]
        public StockAdjustmentStatus StockAdjustmentStatus { get; set; }= StockAdjustmentStatus.Approved;

        // =========================
        // Navigation Properties
        // =========================

        public Stock Stock { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public User Creator { get; set; } = null!;

        public User? Approver { get; set; }
    }
}