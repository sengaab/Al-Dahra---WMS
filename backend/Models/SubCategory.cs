using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SubCategory_Name { get; set; }

        [MaxLength(500)]
        public string? SubCategory_Description { get; set; }

        // =========================
        // Foreign Key
        // =========================

        [Required]
        public int CategoryId { get; set; }

        // =========================
        // Navigation Property
        // =========================

        [ForeignKey(nameof(CategoryId))]
        public Categories Category { get; set; }

        // =========================
        // Products
        // =========================

        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        // =========================
        // Status
        // =========================

        public bool IsActive { get; set; } = true;

        // =========================
        // Dates
        // =========================

        public DateTimeOffset CreatedAt { get; set; }
            = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}

