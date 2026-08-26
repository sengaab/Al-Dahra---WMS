using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    // =====================================================
    // CREATE USER DTO
    // =====================================================

    public class CreateUserDTO
    {
        [MaxLength(50)]
        public string? EmployeeCode { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int RoleId { get; set; }

        public int? DepartmentId { get; set; }

        public bool IsActive { get; set; } = true;
    }


    // =====================================================
    // UPDATE USER DTO
    // =====================================================

    public class UpdateUserDTO
    {
        [MaxLength(50)]
        public string? EmployeeCode { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int RoleId { get; set; }

        public int? DepartmentId { get; set; }

        public bool IsActive { get; set; }
    }


    // =====================================================
    // USER RESPONSE DTO
    // =====================================================

    public class UserResponseDTO
    {
        public Guid UserId { get; set; }

        public string? EmployeeCode { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public string? RoleName { get; set; }

        public int? DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }


    // =====================================================
    // USER ACTIVITY DTO
    // =====================================================

    public class UserActivityDTO
    {
        public long AuditLogId { get; set; }

        public string EntityType { get; set; } = string.Empty;

        public int EntityId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }


    // =====================================================
    // USER PERMISSIONS DTO
    // =====================================================

    public class UserPermissionsDTO
    {
        public Guid UserId { get; set; }

        public int RoleId { get; set; }

        public string? RoleName { get; set; }

        public List<string> Permissions { get; set; }
            = new List<string>();
    }
}