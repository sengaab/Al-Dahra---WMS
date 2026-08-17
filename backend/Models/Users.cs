using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended
    }

    public class Users
    {
        [Key]
        public Guid User_Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string User_Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string User_Email { get; set; } = string.Empty;

        [Required]
        public string User_Password { get; set; } = string.Empty;

        // Email Verification
        public bool EmailConfirmed { get; set; } = false;

        [MaxLength(6)]
        public string? EmailVerificationCode { get; set; }

        public DateTimeOffset? EmailVerificationExpiresAt { get; set; }

        // User Information
        public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdateAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? LoginAt { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Active;

        // Role
        [ForeignKey(nameof(role))]
        public int Role_Id { get; set; }

        public Role role { get; set; } = null!;

        // Navigation Properties
        public List<Report> Reports { get; set; } = new List<Report>();

        public List<ReportSchedule> reportSchedules { get; set; }
            = new List<ReportSchedule>();

        public List<Transaction> transactions { get; set; }
            = new List<Transaction>();

        public List<AuditLog> auditLog { get; set; }
            = new List<AuditLog>();
    }
}