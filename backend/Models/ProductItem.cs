using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class ProductItem
    {
        // =========================
        // Primary Key
        // =========================

        [Key]
        public int ItemId { get; set; }


        // =========================
        // Item Code
        // Unique for each item
        // =========================

        [Required]
        [MaxLength(100)]
        public string ItemCode { get; set; } = string.Empty;


        // =========================
        // Barcode
        // Unique for each item
        // =========================

        [Required]
        [MaxLength(100)]
        public string Barcode { get; set; } = string.Empty;


        // =========================
        // QR Code
        // Unique for each item
        // =========================

        [Required]
        [MaxLength(200)]
        public string QRValue { get; set; } = string.Empty;


        // =========================
        // Product
        // =========================

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;


        // =========================
        // Stock
        // =========================

        [Required]
        public int StockId { get; set; }

        [ForeignKey(nameof(StockId))]
        public Stock Stock { get; set; } = null!;


        // =========================
        // Active
        // =========================

        public bool IsActive { get; set; } = true;


        // =========================
        // Dates
        // =========================

        public DateTimeOffset CreatedAt { get; set; }
            = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}