using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum ReservationStatus
    {
        Pending,
        Active,
        PartiallyFulfilled,
        Fulfilled,
        Released,
        Cancelled,
        Expired
    }
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int RequestItemId { get; set; }

        [Required]
        public int StockId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }

        [Required]
        public Guid ReservedBy { get; set; }

        public DateTimeOffset ReservedAt { get; set; }

        
        [Required]
        public ReservationStatus reservationStatus { get; set; } = ReservationStatus.Released;


        // =========================
        // Navigation Properties
        // =========================

        public StockRequest StockRequest { get; set; } = null!;

        public StockRequestItem RequestItem { get; set; } = null!;

        public Stock Stock { get; set; } = null!;

        public User Reserver { get; set; } = null!;
    }
}