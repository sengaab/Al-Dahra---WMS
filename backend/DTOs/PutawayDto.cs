using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs.Putaways
{
    public class PutawayDto
    {
        public int PutawayId { get; set; }

        public string PutawayNumber { get; set; } = string.Empty;

        public int ReceiptId { get; set; }

        public int WarehouseId { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public PutawayStatus Status { get; set; }

        public List<PutawayItemDto> Items { get; set; }
            = new List<PutawayItemDto>();
    }
    public class CreatePutawayDto
    {
        [Required]
        [MaxLength(50)]
        public string PutawayNumber { get; set; } = string.Empty;

        [Required]
        public int ReceiptId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

      
    }
    public class UpdatePutawayDto
    {
        [Required]
        [MaxLength(50)]
        public string PutawayNumber { get; set; } = string.Empty;

        [Required]
        public int WarehouseId { get; set; }
    }
    public class PutawayItemDto
    {
        public int PutawayItemId { get; set; }

        public int PutawayId { get; set; }

        public int ReceiptItemId { get; set; }

        public int ProductId { get; set; }

        public int LocationId { get; set; }

        public decimal Quantity { get; set; }

        public int StockId { get; set; }
    }
    public class CreatePutawayItemDto
    {
        [Required]
        public int ReceiptItemId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public int StockId { get; set; }
    }

    public class UpdatePutawayItemDto
    {
        [Required]
        public int LocationId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public int StockId { get; set; }
    }

}