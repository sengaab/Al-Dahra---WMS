using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class UpdateBinDTO
    {
        [Required]
        [MaxLength(50)]
        public string Bin_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Bin_Code { get; set; }

        [MaxLength(500)]
        public string? Bin_Description { get; set; }

        public bool? IsActive { get; set; }
    }
}