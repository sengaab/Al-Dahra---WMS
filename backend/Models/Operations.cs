using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum OperationType
    {
        StockIn,
        StockOut,
        Transfer,
        Adjustment
    }

    public class Operations
    {
        [Key]
        public int Operation_Id { get; set; }

        [Required]
        public OperationType OperationType { get; set; }
            = OperationType.StockIn;


        // =========================
        // Product
        // =========================

        [Required]
        public int Product_Id { get; set; }

        [ForeignKey(nameof(Product_Id))]
        public Product Product { get; set; } = null!;


        // =========================
        // Quantity
        // =========================

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }


        // =========================
        // Unit
        // =========================

        [Required]
        public int Unit_Id { get; set; }

        [ForeignKey(nameof(Unit_Id))]
        public Unit Unit { get; set; } = null!;


        // =========================
        // From Bin
        // =========================

        public int? FromBinId { get; set; }

        [ForeignKey(nameof(FromBinId))]
        public Bin? FromBin { get; set; }


        // =========================
        // To Bin
        // =========================

        public int? ToBinId { get; set; }

        [ForeignKey(nameof(ToBinId))]
        public Bin? ToBin { get; set; }


        // =========================
        // User
        // =========================

        [Required]
        public Guid User_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public Users User { get; set; } = null!;


        // =========================
        // Notes
        // =========================

        [MaxLength(500)]
        public string? Notes { get; set; }


        // =========================
        // Created At
        // =========================

        public DateTimeOffset CreateAt { get; set; }
            = DateTimeOffset.UtcNow;
    }
}