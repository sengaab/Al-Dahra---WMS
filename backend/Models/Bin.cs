using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class Bin
    {
        [Key]
        public int Bin_Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Bin_Name { get; set; }=string.Empty;
        [MaxLength(50)]
        public string? Bin_Code { get; set;} = string.Empty;
        [MaxLength(500)]
        public string? Bin_Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(Shelf))]
        public int Shelf_Id { get; set; }
        public Shelf Shelf { get; set; } = null!;
        public List<Stock> Stocks { get; set; }=new List<Stock>();
        public List<Transaction> Fromtransactions { get; set; } = new List<Transaction>();
        public List<Transaction> Totransactions { get; set; } = new List<Transaction>();
       
    }
}
