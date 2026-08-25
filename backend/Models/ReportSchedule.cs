using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class ReportSchedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Frequency { get; set; } = string.Empty;

        public string? Recipients { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset? NextRunAt { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public Report Report { get; set; } = null!;
    }
}