using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class CreateRowDTO
    {
        [Required]
        [MaxLength(50)]
        public string Row_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Row_Code { get; set; }

        [MaxLength(500)]
        public string? Row_Description { get; set; }

        [Required]
        public int Room_Id { get; set; }
    }
}