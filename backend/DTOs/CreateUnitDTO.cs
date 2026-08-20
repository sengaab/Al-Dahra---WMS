using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class CreateUnitDTO
    {
        [Required]
        public string Unit_Name { get; set; }

        [Required]
        public string Unit_Symbol { get; set; }
    }
}