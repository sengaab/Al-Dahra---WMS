using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace whm.Models
{
    public class Alias
    {
        [Key]
        public int AliasId { get; set; }

        [Required]
        [MaxLength(200)]
        public string AliasName { get; set; } = string.Empty;

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }
            = DateTimeOffset.UtcNow;
    }
}