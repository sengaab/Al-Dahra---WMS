using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class Warehouse
    {
        [Key]
        public int Warehouse_Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Warehouse_Name { get; set; }=string.Empty;
        [MaxLength(50)]
        public string? Warehouse_Code  { get; set; }=string.Empty;
        [MaxLength(500)]
        public string? Warehouse_Description { get; set; }=string.Empty;
        public bool IsActive { get; set; } = true;
        //Navigation
        public List<Room>Rooms { get; set; }=new List<Room>();
        public List<Report> reports { get; set; }=new List<Report>();
        public List<ReportSchedule> reportSchedules { get; set; } = new List<ReportSchedule>();
        [ForeignKey(nameof(Site))]
        public int Site_Id { get; set; }

        public Site Site { get; set; } = null!;


    }
}
