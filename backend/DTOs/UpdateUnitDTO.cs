using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class UpdateUnitDTO
    {
        [Required]
        public string Unit_Name { get; set; }

        [Required]
        public string Unit_Symbol { get; set; }

        public bool IsActive { get; set; }
    }
}