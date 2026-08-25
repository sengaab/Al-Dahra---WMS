using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public User Creator { get; set; } = null!;

        public List<ReportSchedule> Schedules { get; set; }
            = new List<ReportSchedule>();
    }
}