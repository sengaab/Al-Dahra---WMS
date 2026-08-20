using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public enum TransactionType
    {
        StockIn,
        StockOut,
        Transfar,
        Adjustment

    }
    public class Transaction
    {
        [Key]
        public int transaction_Id { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }= TransactionType.StockIn;
        [Required]
        public int Product_Id { get; set; }
        [ForeignKey(nameof(Product_Id))]
        public Product Product { get; set; } = null!;
        [Required]
        [Range(0.01,double.MaxValue)]
        public decimal Quantity { get; set; }
        [Required]
        public int Unit_Id { get; set; }
        [ForeignKey(nameof(Unit_Id))]
        public Unit Unit { get; set; }=null!;
        public int ? FromBinId { get; set; }
        [ForeignKey(nameof(FromBinId))]
        public Bin? FromBin { get; set; }
        public int? ToBinId { get; set; }
        [ForeignKey(nameof(ToBinId))]
        public Bin? ToBin { get; set; }
        [Required]
        public Guid User_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public Users User { get; set; } = null!;
        [MaxLength(500)]
        public string? Notes { get; set; }
        public DateTimeOffset CreateAt { get; set; }=DateTimeOffset.UtcNow;
       

    }
}
