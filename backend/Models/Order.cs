using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }


        // =========================
        // Order
        // =========================

        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;


        // =========================
        // Product
        // =========================

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;


        // =========================
        // Quantity
        // =========================

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }


        // =========================
        // Unit Price
        // =========================

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }


        // =========================
        // Tax Rate
        // =========================

        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxRate { get; set; }


        // =========================
        // Total Price
        // =========================

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }


        // =========================
        // Received Quantity
        // =========================

        [Range(0, double.MaxValue)]
        public decimal ReceivedQuantity { get; set; } = 0;


        // =========================
        // Notes
        // =========================

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}