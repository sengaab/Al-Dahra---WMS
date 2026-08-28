using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs
{
    // =====================================================
    // CREATE STOCK RETURN
    // =====================================================

    public class CreateStockReturnDTO
    {
        [Required]
        public int IssueId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public Guid ReturnedBy { get; set; }

        public string? Reason { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateStockReturnItemDTO> Items { get; set; }
            = new List<CreateStockReturnItemDTO>();
    }


    // =====================================================
    // CREATE RETURN ITEM
    // =====================================================

    public class CreateStockReturnItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int StockId { get; set; }

        [Required]
        [Range(0.001, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string Condition { get; set; } = string.Empty;
    }


    // =====================================================
    // RESPONSE
    // =====================================================

    public class StockReturnResponseDTO
    {
        public int ReturnId { get; set; }

        public string ReturnNumber { get; set; } = string.Empty;

        public int IssueId { get; set; }

        public int WarehouseId { get; set; }

        public int DepartmentId { get; set; }

        public Guid ReturnedBy { get; set; }

        public DateTimeOffset ReturnedAt { get; set; }

        public StockReturnStatus StockReturnStatus { get; set; }

        public string? Reason { get; set; }

        public List<StockReturnItemResponseDTO> Items { get; set; }
            = new List<StockReturnItemResponseDTO>();
    }


    // =====================================================
    // ITEM RESPONSE
    // =====================================================

    public class StockReturnItemResponseDTO
    {
        public int ReturnItemId { get; set; }

        public int ProductId { get; set; }

        public int StockId { get; set; }

        public decimal Quantity { get; set; }

        public string Condition { get; set; } = string.Empty;
    }


    // =====================================================
    // WORKFLOW ACTION
    // =====================================================

    public class StockReturnActionDTO
    {
        [Required]
        public Guid UserId { get; set; }
    }
}