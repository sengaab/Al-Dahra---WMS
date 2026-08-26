using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    // =====================================================
    // CREATE PICK LIST DTO
    // =====================================================

    public class CreatePickListDTO
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        public Guid? AssignedTo { get; set; }
    }


    // =====================================================
    // UPDATE PICK LIST DTO
    // =====================================================

    public class UpdatePickListDTO
    {
        public Guid? AssignedTo { get; set; }

        [Required]
        public string PickListStatus { get; set; } = "Draft";
    }


    // =====================================================
    // ASSIGN PICK LIST DTO
    // =====================================================

    public class AssignPickListDTO
    {
        [Required]
        public Guid AssignedTo { get; set; }
    }


    // =====================================================
    // PICK LIST RESPONSE DTO
    // =====================================================

    public class PickListResponseDTO
    {
        public int PickListId { get; set; }

        public string PickNumber { get; set; } = string.Empty;

        public int RequestId { get; set; }

        public string? RequestNumber { get; set; }

        public int WarehouseId { get; set; }

        public string? WarehouseName { get; set; }

        public Guid? AssignedTo { get; set; }

        public string? AssigneeName { get; set; }

        public string PickListStatus { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? StartedAt { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }

        public List<PickItemResponseDTO> Items { get; set; }
            = new List<PickItemResponseDTO>();
    }


    // =====================================================
    // PICK ITEM RESPONSE DTO
    // =====================================================

    public class PickItemResponseDTO
    {
        public int PickItemId { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public int StockId { get; set; }

        public int LocationId { get; set; }

        public decimal RequestedQuantity { get; set; }

        public decimal PickedQuantity { get; set; }

        public string PickItemStatus { get; set; } = string.Empty;
    }
}