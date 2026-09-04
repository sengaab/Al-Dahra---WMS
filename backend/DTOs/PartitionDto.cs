using System.ComponentModel.DataAnnotations;

namespace whm.DTOs.Partition
{
    public class PartitionDto
    {
        public int PartitionId { get; set; }

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int BinsCount { get; set; }
    }

    public class CreatePartitionDto
    {
        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class UpdatePartitionDto
    {
        public int? WarehouseId { get; set; }

        [MaxLength(50)]
        public string? Code { get; set; }

        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }

    public class PartitionSummaryDto
    {
        public int PartitionId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public int BinsCount { get; set; }

        public bool IsActive { get; set; }
    }
}