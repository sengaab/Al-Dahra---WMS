using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum ScheduleFrequency
    {
        Daily,
        Weekly,
        Monthly
    }
    public class ReportSchedule
    {
        [Key]
        public int ReportSchedule_Id { get; set; }
        [Required]
        public ReportType ReportType { get; set; }
        [Required]
        public ScheduleFrequency Frequency { get; set; }
        [Required]
        public TimeSpan RunAt { get; set; }
        public bool IsActive { get; set; }=true;
        [ForeignKey(nameof(Users))]
        public Guid craeteByUserId { get; set; }
        public Users CreateByUser { get; set; }
        [ForeignKey(nameof(Warehouse))]
        public int? Warehouse_Id { get; set; }
        public Warehouse? Warehouses { get; set; }
        [ForeignKey(nameof(Product))]
        public int? Product_Id { get; set; }
        public Product? Products { get; set; }
        public DateTimeOffset? LastRunAt { get; set; }
        public DateTimeOffset? NextRunAt { get; set; }
        public DateTimeOffset? UpdateAt { get; set; }
        public DateTimeOffset CreateAt { get; set; }=DateTimeOffset.UtcNow;

    }
}
