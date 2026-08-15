using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class Room
    {
        [Key]
        public int Room_Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Room_Name { get; set; }=string.Empty;
        [MaxLength(50)]
        public string? Room_Code { get; set; }=string.Empty;
        [MaxLength(500)]
        public string? Room_Description { get; set; }=string.Empty;
        public bool IsActive { get; set; }=true;
        [ForeignKey(nameof(Warehouse))]
        public int Warehouse_Id { get; set; }
        public Warehouse Warehouse { get; set; } = null!;
        //Navigation
        public List<Row>Rows { get; set; }=new List<Row>();
    }
}
