using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class CreateRoleDTO
    {
        [Required]
        [MaxLength(50)]
        public string Role_Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }
    }
}