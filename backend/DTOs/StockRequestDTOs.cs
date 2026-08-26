using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs
{
    // =====================================================
    // CREATE STOCK REQUEST
    // =====================================================

    public class CreateStockRequestDTO
    {
        [Required]
        [MaxLength(50)]
        public string RequestNumber { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [Required]
        public Guid RequestedBy { get; set; }

        [Required]
        [MaxLength(30)]
        public string Priority { get; set; } = "Normal";

        public StockRequestStatus StockRequestStatus { get; set; }
            = StockRequestStatus.Draft;
    }


    // =====================================================
    // UPDATE STOCK REQUEST
    // =====================================================

    public class UpdateStockRequestDTO
    {
        [Required]
        [MaxLength(50)]
        public string RequestNumber { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Priority { get; set; } = "Normal";

        public StockRequestStatus StockRequestStatus { get; set; }
    }


    // =====================================================
    // STOCK REQUEST RESPONSE
    // =====================================================

    public class StockRequestResponseDTO
    {
        public int RequestId { get; set; }

        public string RequestNumber { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        public int SiteId { get; set; }

        public string? SiteName { get; set; }

        public Guid RequestedBy { get; set; }

        public string? RequesterName { get; set; }

        public Guid? ApprovedBy { get; set; }

        public string? ApproverName { get; set; }

        public string Priority { get; set; } = string.Empty;

        public DateTimeOffset RequestedAt { get; set; }

        public DateTimeOffset? ApprovedAt { get; set; }

        public StockRequestStatus StockRequestStatus { get; set; }

        public List<StockRequestItemResponseDTO> Items { get; set; }
            = new List<StockRequestItemResponseDTO>();
    }


    // =====================================================
    // CREATE STOCK REQUEST ITEM
    // =====================================================

    public class CreateStockRequestItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal RequestedQuantity { get; set; }
    }


    // =====================================================
    // UPDATE STOCK REQUEST ITEM
    // =====================================================

    public class UpdateStockRequestItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal RequestedQuantity { get; set; }
    }


    // =====================================================
    // STOCK REQUEST ITEM RESPONSE
    // =====================================================

    public class StockRequestItemResponseDTO
    {
        public int RequestItemId { get; set; }

        public int RequestId { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? SKU { get; set; }

        public decimal RequestedQuantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        public decimal IssuedQuantity { get; set; }

        public decimal RemainingQuantity { get; set; }
    }
}