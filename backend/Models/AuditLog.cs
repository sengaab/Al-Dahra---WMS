using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace whm.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLog_Id { get; set; }
        [Required]
        public Guid User_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public Users Users { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        //What the user did
        public string Action { get; set; }=string.Empty;
        [MaxLength(100)]
        //the affected entity
        public string EntityName { get; set; }
        //id of the affected record
        public string? EntityId { get; set; }
        [MaxLength(1000)]
        public string?details { get; set; }
        public DateTimeOffset CreateAt { get; set; }=DateTimeOffset.UtcNow;

    }
}
