using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class Stock
    {
        [Key]
        public int Stock_Id { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        public decimal Quantity { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; }= DateTime.Now;
        public DateTime LastUpdatedAt { get; set; }=DateTime.Now;
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        [Required]
        public int Bin_Id { get; set; }
        [ForeignKey(nameof(Bin_Id))]
        public Bin Bin { get; set; }



        

    }
}
