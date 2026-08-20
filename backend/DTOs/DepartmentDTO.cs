using System.ComponentModel.DataAnnotations;

namespace whm.DTOs.Department
{
    public class CreateDepartmentDTO
    {
        [Required]
        [StringLength(100)]
        public string Department_Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }


    public class UpdateDepartmentDTO
    {
        [Required]
        [StringLength(100)]
        public string Department_Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}