using System.ComponentModel.DataAnnotations;

namespace whm.DTOs.Sites
{
    public class SiteDto
    {
        public int SiteId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int WarehouseCount { get; set; }
    }
    public class SiteCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
        public class SiteUpdateDto
        {
            [Required]
            [MaxLength(50)]
            public string Code { get; set; } = string.Empty;

            [Required]
            [MaxLength(150)]
            public string Name { get; set; } = string.Empty;

            [MaxLength(500)]
            public string? Description { get; set; }

            public bool IsActive { get; set; }
        }
    public class SiteWarehouseDto
    {
        public int WarehouseId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
    public class SiteInventoryDto
    {
        public int ProductId { get; set; }

        public string SKU { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = string.Empty;

        public int BinId { get; set; }

        public string BinName { get; set; } = string.Empty;
    }
    public class SiteStatsDto
    {
        public int SiteId { get; set; }

        public string SiteCode { get; set; } = string.Empty;

        public string SiteName { get; set; } = string.Empty;

        public int WarehouseCount { get; set; }

        public int ProductCount { get; set; }

        public int BinCount { get; set; }

        public decimal TotalQuantity { get; set; }
    }

}