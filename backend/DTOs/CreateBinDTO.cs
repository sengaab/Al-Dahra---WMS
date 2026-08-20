using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class CreateBinDTO
    {
        [Required]
        [MaxLength(50)]
        public string Bin_Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Bin_Code { get; set; }

        [MaxLength(500)]
        public string? Bin_Description { get; set; }

        [Required]
        public int Shelf_Id { get; set; }
    }
}