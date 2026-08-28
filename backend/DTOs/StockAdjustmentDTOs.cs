using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs
{
    // =====================================================
    // CREATE
    // =====================================================

    public class CreateStockAdjustmentDTO
    {
        [Required]
        public int StockId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal AdjustmentQuantity { get; set; }

        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public Guid CreatedBy { get; set; }
    }


    // =====================================================
    // RESPONSE
    // =====================================================

    public class StockAdjustmentResponseDTO
    {
        public int AdjustmentId { get; set; }

        public string AdjustmentNumber { get; set; } = string.Empty;

        public int StockId { get; set; }

        public int ProductId { get; set; }

        public decimal PreviousQuantity { get; set; }

        public decimal AdjustmentQuantity { get; set; }

        public decimal NewQuantity { get; set; }

        public string Reason { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public StockAdjustmentStatus StockAdjustmentStatus { get; set; }
    }


    // =====================================================
    // APPROVE / REJECT
    // =====================================================

    public class StockAdjustmentActionDTO
    {
        [Required]
        public Guid UserId { get; set; }
    }
}