using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
     public enum StockCountStatus
        {
            Draft,
            Scheduled,
            Assigned,
            InProgress,
            PartiallyCounted,
            Counted,
            PendingApproval,
            Approved,
            Rejected,
            Cancelled,
            Completed
     }
    
    public class StockCount
    {
        [Key]
        public int StockCountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CountNumber { get; set; } = string.Empty;

        [Required]
        public int WarehouseId { get; set; }

        public int? LocationId { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        [Required]
        [MaxLength(50)]
        public StockCountStatus stockCountStatus { get; set; }= StockCountStatus.Draft;


        public DateTimeOffset CountDate { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Warehouse Warehouse { get; set; } = null!;

        public Location? Location { get; set; }

        public User Creator { get; set; } = null!;

        public User? Approver { get; set; }

        public List<StockCountItem> Items { get; set; }
            = new List<StockCountItem>();
    }
}