using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class StockRequestItem
    {
        [Key]
        public int RequestItemId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal RequestedQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ReservedQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal IssuedQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal RemainingQuantity { get; set; } = 0;


        // =========================
        // Navigation Properties
        // =========================

        public StockRequest StockRequest { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public List<Reservation> Reservations { get; set; }
            = new List<Reservation>();
    }
}
