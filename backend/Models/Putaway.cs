using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum PutawayStatus
    {
        Pending,
        Assigned,
        InProgress,
        PartiallyCompleted,
        Completed,
        Cancelled
    }
    public class Putaway
    {
        [Key]
        public int PutawayId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PutawayNumber { get; set; } = string.Empty;

        [Required]
        public int ReceiptId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        
        [Required]
        public PutawayStatus StatusStatus { get; set; }=PutawayStatus.Pending;


        // =========================
        // Navigation Properties
        // =========================

        public Receipt Receipt { get; set; } = null!;

        public Warehouse Warehouse { get; set; } = null!;

        public User Creator { get; set; } = null!;

        public List<PutawayItem> Items { get; set; }
            = new List<PutawayItem>();
    }
}