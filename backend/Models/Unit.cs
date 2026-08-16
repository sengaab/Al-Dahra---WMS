using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Unit
    {
        [Key]
        public int Unit_Id { get; set; }
        [Required]
        public string Unit_Name { get; set; } =String.Empty;
        [Required]
        public string Unit_Symbol { get; set;} =String.Empty;
        public bool IsActive { get; set; }=true;
        public List<Product> Products { get; set; }=new List<Product>();
        public List<Transaction> transactions { get; set; } = new List<Transaction>();




    }
}
