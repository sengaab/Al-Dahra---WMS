using System.ComponentModel.DataAnnotations;

namespace whm.Models
{
    public class Department
    {
        [Key]
        public int Department_Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Department_Name { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreateAt { get; set; }= DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdateAt { get;set; }= DateTimeOffset.UtcNow;
        public List<Categories> Categories { get; set; }=new List<Categories>();
    }
}
