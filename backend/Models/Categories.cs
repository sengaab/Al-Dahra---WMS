using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class Categories
    {
        [Key]
        public int Category_Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category_Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        // Navigation Property
        public List<Product> Products { get; set; }=new List<Product>();
        [ForeignKey(nameof(Department))]
        public int Department_Id { get; set; }
        public Department Department { get; set; } = null!;
    }
}

