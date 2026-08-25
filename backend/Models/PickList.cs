using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum PickListStatus
    {
        Pending,
        Assigned,
        InProgress,
        PartiallyPicked,
        Picked,
        Cancelled,
        Completed
    }
    public class PickList
    {
        [Key]
        public int PickListId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PickNumber { get; set; } = string.Empty;

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        public Guid? AssignedTo { get; set; }

       
        [Required]
        public PickListStatus PickListStatus { get; set; }=PickListStatus.Pending;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public StockRequest StockRequest { get; set; } = null!;

        public Warehouse Warehouse { get; set; } = null!;

        public User? Assignee { get; set; }

        public List<PickItem> Items { get; set; }
            = new List<PickItem>();
    }
}