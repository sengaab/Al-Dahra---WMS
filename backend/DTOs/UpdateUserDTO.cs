using System.ComponentModel.DataAnnotations;

namespace whm.DTOs
{
    public class UpdateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string User_Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string User_Email { get; set; } = string.Empty;

        [Required]
        public int Role_Id { get; set; }
    }
}