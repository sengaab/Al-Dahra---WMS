using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs
{
    public class CreateTransactionDTO
    {
        [Required]
        public int Product_Id { get; set; }

        [Required]
        [Range(0.001, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        public int Unit_Id { get; set; }

        [Required]
        public int TransactionType { get; set; }

        public int? FromBinId { get; set; }

        public int? ToBinId { get; set; }

        public string? Notes { get; set; }
    }
}