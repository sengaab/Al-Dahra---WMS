using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs
{
    // =====================================================
    // CREATE
    // =====================================================

    public class CreateStockCountDTO
    {
        [Required]
        public int WarehouseId { get; set; }

        public int? LocationId { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CountDate { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateStockCountItemDTO> Items { get; set; }
            = new();
    }


    public class CreateStockCountItemDTO
    {
        [Required]
        public int StockId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }


    // =====================================================
    // UPDATE
    // =====================================================

    public class UpdateStockCountDTO
    {
        [Required]
        public int WarehouseId { get; set; }

        public int? LocationId { get; set; }

        public DateTimeOffset CountDate { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateStockCountItemDTO> Items { get; set; }
            = new();
    }


    // =====================================================
    // RESPONSE
    // =====================================================

    public class StockCountResponseDTO
    {
        public int StockCountId { get; set; }

        public string CountNumber { get; set; } = string.Empty;

        public int WarehouseId { get; set; }

        public int? LocationId { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        public StockCountStatus StockCountStatus { get; set; }

        public DateTimeOffset CountDate { get; set; }

        public List<StockCountItemResponseDTO> Items { get; set; }
            = new();
    }


    public class StockCountItemResponseDTO
    {
        public int StockCountItemId { get; set; }

        public int StockId { get; set; }

        public int ProductId { get; set; }

        public decimal ExpectedQuantity { get; set; }

        public decimal CountedQuantity { get; set; }

        public decimal Variance { get; set; }

        public string? Reason { get; set; }
    }


    // =====================================================
    // COUNT ITEM
    // =====================================================

    public class CountStockCountItemDTO
    {
        [Required]
        [Range(typeof(decimal), "0", "999999999999")]
        public decimal CountedQuantity { get; set; }

        public string? Reason { get; set; }
    }


    // =====================================================
    // APPROVE
    // =====================================================

    public class ApproveStockCountDTO
    {
        [Required]
        public Guid ApprovedBy { get; set; }
    }
}