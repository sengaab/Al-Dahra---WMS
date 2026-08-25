using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum StockReturnStatus
    {
        Pending,
        PendingInspection,
        Approved,
        Rejected,
        Accepted,
        PartiallyAccepted,
        Quarantined,
        Completed,
        Cancelled
    }

    public class StockReturn
    {
        [Key]
        public int ReturnId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ReturnNumber { get; set; } = string.Empty;

        [Required]
        public int IssueId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public Guid ReturnedBy { get; set; }

        public DateTimeOffset ReturnedAt { get; set; }

        [Required]
        [MaxLength(50)]
      public StockReturnStatus stockReturnStatus { get; set; }= StockReturnStatus.Pending;

        public string? Reason { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public StockIssue StockIssue { get; set; } = null!;

        public Warehouse Warehouse { get; set; } = null!;

        public Department Department { get; set; } = null!;

        public User Returner { get; set; } = null!;

        public List<StockReturnItem> Items { get; set; }
            = new List<StockReturnItem>();
    }
}