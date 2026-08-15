using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class Shelf
    {
        [Key]
        public int Shelf_Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Shelf_Name { get; set; }=String.Empty;
        [MaxLength(50)]
        public string? Shelf_Code { get; set; }=String.Empty;
        [MaxLength(500)]
        public string? Shelf_Description { get; set; }=String.Empty;
        public bool IsActive { get; set; } = true!;
        [ForeignKey(nameof(Row))]
        public int Row_Id { get; set; }
        public Row Row { get; set; }=null!;
        public List<Bin>Bins { get; set; }=new List<Bin>();








    }
}
