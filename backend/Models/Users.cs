using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [MaxLength(50)]
        public string? EmployeeCode { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public int? DepartmentId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation
        public Role Role { get; set; } = null!;

        public Department? Department { get; set; }
    }
}