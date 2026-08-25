using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Abbreviation { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; }
            = new List<Product>();
    }
}