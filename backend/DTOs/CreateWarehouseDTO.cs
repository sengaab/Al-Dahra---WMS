using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class CreateWarehouseDTO
    {
        [Required]
        [MaxLength(50)]
        public string Warehouse_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Warehouse_Code { get; set; }

        [MaxLength(500)]
        public string? Warehouse_Description { get; set; }
    }
}