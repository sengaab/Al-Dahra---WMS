using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs
{
    public class CreateStockIssueDTO
    {
        [Required]
        public int RequestId { get; set; }

        public int? PickListId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public Guid IssuedBy { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateStockIssueItemDTO> Items { get; set; }
            = new();
    }


    public class CreateStockIssueItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0.001, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        public int StockId { get; set; }
    }


    public class StockIssueResponseDTO
    {
        public int IssueId { get; set; }

        public string IssueNumber { get; set; } = string.Empty;

        public int RequestId { get; set; }

        public int? PickListId { get; set; }

        public int WarehouseId { get; set; }

        public int DepartmentId { get; set; }

        public Guid IssuedBy { get; set; }

        public DateTimeOffset IssuedAt { get; set; }

        public StockIssueStatus StockIssueStatus { get; set; }

        public List<StockIssueItemResponseDTO> Items { get; set; }
            = new();
    }


    public class StockIssueItemResponseDTO
    {
        public int IssueItemId { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public int StockId { get; set; }
    }


    public class StockIssueActionDTO
    {
        [Required]
        public Guid UserId { get; set; }
    }
}