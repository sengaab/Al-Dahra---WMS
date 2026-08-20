using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class UpdateRoomDTO
    {
        [Required]
        [MaxLength(50)]
        public string Room_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Room_Code { get; set; }

        [MaxLength(500)]
        public string? Room_Description { get; set; }

        public bool IsActive { get; set; }
    }
}