using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using whm.Models;
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
        public string User_Name { get; set; }
        [Required]
     
        public string User_Email { get; set; }=string.Empty;
        [Required]
        public string User_Password { get; set;} = string.Empty;

        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset UpdateAt { get; set; } = DateTime.Now;
        public DateTimeOffset? LoginAt { get; set; }
        public UserStatus Status { get; set; }
        [ForeignKey("Role")]
        public int Role_Id { get; set; }
        public Role role { get; set; }
        public List<Report> Reports { get; set; }=new List<Report>();
        public List<ReportSchedule> reportSchedules { get; set; } = new List<ReportSchedule>();
        public List<Transaction> transactions { get; set; } = new List<Transaction>();
        public List<AuditLog> auditLog { get; set; }= new List<AuditLog>();


    }
}
