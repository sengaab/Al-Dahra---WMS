using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    // =====================================================
    // CREATE ROLE DTO
    // =====================================================

    public class CreateRoleDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }


    // =====================================================
    // UPDATE ROLE DTO
    // =====================================================

    public class UpdateRoleDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }


    // =====================================================
    // ROLE RESPONSE DTO
    // =====================================================

    public class RoleResponseDTO
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int UsersCount { get; set; }
    }


    // =====================================================
    // ROLE PERMISSIONS DTO
    // =====================================================

    public class RolePermissionsDTO
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public List<string> Permissions { get; set; }
            = new List<string>();
    }


    // =====================================================
    // UPDATE ROLE PERMISSIONS DTO
    // =====================================================

    public class UpdateRolePermissionsDTO
    {
        public List<string> Permissions { get; set; }
            = new List<string>();
    }
}
