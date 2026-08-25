using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class AuditLog
    {
        [Key]
        public long AuditLogId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string EntityType { get; set; } = string.Empty;

        [Required]
        public int EntityId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTimeOffset CreatedAt { get; set; }


        // =========================
        // Navigation Properties
        // =========================

        public User User { get; set; } = null!;
    }
}