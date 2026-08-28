using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    // =====================================================
    // CREATE DEPARTMENT DTO
    // =====================================================

    public class CreateDepartmentDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Code { get; set; }

        public bool IsActive { get; set; } = true;
    }


    // =====================================================
    // UPDATE DEPARTMENT DTO
    // =====================================================

    public class UpdateDepartmentDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Code { get; set; }

        public bool IsActive { get; set; }
    }


    // =====================================================
    // DEPARTMENT RESPONSE DTO
    // =====================================================

    public class DepartmentResponseDTO
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int UsersCount { get; set; }

        public int RequestsCount { get; set; }
    }


    // =====================================================
    // DEPARTMENT USER DTO
    // =====================================================

    public class DepartmentUserDTO
    {
        public Guid UserId { get; set; }

        public string? EmployeeCode { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public string? RoleName { get; set; }

        public bool IsActive { get; set; }
    }


    // =====================================================
    // DEPARTMENT REQUEST DTO
    // =====================================================

    public class DepartmentRequestDTO
    {
        public int RequestId { get; set; }

        public string? RequestNumber { get; set; }

        public Guid RequestedBy { get; set; }

        public string? RequesterName { get; set; }

        public int DepartmentId { get; set; }

        public string? Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}