using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
   

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

        

        // User Information
        public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdateAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? LoginAt { get; set; }

        

       // Role
        [ForeignKey(nameof(role))]
        public int Role_Id { get; set; }

        public Role role { get; set; } = null!;

        // Navigation Properties
        public List<Report> Reports { get; set; } = new List<Report>();

        public List<ReportSchedule> reportSchedules { get; set; }
            = new List<ReportSchedule>();

        public List<Operations> Operations { get; set; }
            = new List<Operations>();

        public List<AuditLog> auditLog { get; set; }
            = new List<AuditLog>();
    }
}