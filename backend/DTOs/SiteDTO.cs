using System.ComponentModel.DataAnnotations;

namespace whm.DTOs.Site
{
    public class CreateSiteDTO
    {
        [Required]
        [StringLength(100)]
        public string Site_Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Site_Code { get; set; }

        [StringLength(500)]
        public string? Site_Description { get; set; }
    }


    public class UpdateSiteDTO
    {
        [Required]
        [StringLength(100)]
        public string Site_Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Site_Code { get; set; }

        [StringLength(500)]
        public string? Site_Description { get; set; }

        public bool IsActive { get; set; }
    }
}
