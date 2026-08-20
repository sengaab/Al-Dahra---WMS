using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class UpdateWarehouseDTO
    {
        [Required]
        [MaxLength(50)]
        public string Warehouse_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Warehouse_Code { get; set; }

        [MaxLength(500)]
        public string? Warehouse_Description { get; set; }
        public int Site_Id { get; set; }
    }
}