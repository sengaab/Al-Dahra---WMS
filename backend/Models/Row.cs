using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class Row
    {
        [Key]
        public int Row_Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Row_Name { get; set; }=String.Empty;
        [MaxLength(50)]
        public string? Row_Code { get; set;} = String.Empty;
        [MaxLength(500)]
        public string? Row_Description { get; set; } = String.Empty;
        public bool IsActive { get; set; }=true;
        [ForeignKey(nameof(Room))]
        public int Room_Id { get; set; }
        public Room Room { get; set; } = null!;
            public List<Shelf>Shelves { get; set; }=new List<Shelf>();

    }
}
