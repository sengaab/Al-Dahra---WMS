using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public enum InspectionStatus
    {
        Pending,
        InProgress,
        Passed,
        Failed,
        Quarantined,
        Cancelled
    }
    public class Inspection
    {
        [Key]
        public int InspectionId { get; set; }

        [Required]
        public int ReceiptItemId { get; set; }

        [Required]
        public Guid InspectedBy { get; set; }

        public DateTimeOffset InspectedAt { get; set; }

       
        [Required]
        public InspectionStatus InspectionStatus { get; set; }=InspectionStatus.Pending;

        public string? Notes { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public ReceiptItem ReceiptItem { get; set; } = null!;

        public User Inspector { get; set; } = null!;
    }
}