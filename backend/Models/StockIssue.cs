using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum StockIssueStatus
    {
        Pending,
        Ready,
        Issued,
        PartiallyIssued,
        Completed,
        Cancelled
    }
    public class StockIssue
    {
        [Key]
        public int IssueId { get; set; }

        [Required]
        [MaxLength(50)]
        public string IssueNumber { get; set; } = string.Empty;

        [Required]
        public int RequestId { get; set; }

        public int? PickListId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public Guid IssuedBy { get; set; }

        public DateTimeOffset IssuedAt { get; set; }

        [Required]
        public StockIssueStatus StockIssueStatus { get; set; }= StockIssueStatus.Pending;


        // =========================
        // Navigation Properties
        // =========================

        public StockRequest StockRequest { get; set; } = null!;

        public PickList? PickList { get; set; }

        public Warehouse Warehouse { get; set; } = null!;

        public Department Department { get; set; } = null!;

        public User Issuer { get; set; } = null!;

        public List<StockIssueItem> Items { get; set; }
            = new List<StockIssueItem>();
    }
}