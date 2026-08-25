using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum StockRequestStatus
    {
        Draft,
        Submitted,
        PendingApproval,
        Approved,
        Rejected,
        PartiallyReserved,
        Reserved,
        Picking,
        PartiallyIssued,
        Issued,
        Completed,
        Cancelled
    }
    public class StockRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RequestNumber { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [Required]
        public Guid RequestedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        [Required]
        [MaxLength(30)]
        public string Priority { get; set; } = "Normal";

        

        public DateTimeOffset RequestedAt { get; set; }

        public DateTimeOffset? ApprovedAt { get; set; }
        [Required]
        public StockRequestStatus StockRequestStatus { get; set; }= StockRequestStatus.Completed;


        // =========================
        // Navigation Properties
        // =========================

        public Department Department { get; set; } = null!;

        public Site Site { get; set; } = null!;

        public User Requester { get; set; } = null!;

        public User? Approver { get; set; }

        public List<StockRequestItem> Items { get; set; }
            = new List<StockRequestItem>();

        public List<Reservation> Reservations { get; set; }
            = new List<Reservation>();

        public List<PickList> PickLists { get; set; }
            = new List<PickList>();

        public List<StockIssue> StockIssues { get; set; }
            = new List<StockIssue>();
    }
}