using System.ComponentModel.DataAnnotations;
using whm.Models;

namespace whm.DTOs.Inspections
{
    public class InspectionDto
    {
        public int InspectionId { get; set; }

        public int ReceiptItemId { get; set; }

        public Guid InspectedBy { get; set; }

        public DateTimeOffset InspectedAt { get; set; }

        public InspectionStatus InspectionStatus { get; set; }

        public string? Notes { get; set; }
    }

    public class CreateInspectionDto
    {
        [Required]
        public int ReceiptItemId { get; set; }

      

        public DateTimeOffset? InspectedAt { get; set; }

        public string? Notes { get; set; }
    }
    public class UpdateInspectionDto
    {
        [Required]
        public InspectionStatus InspectionStatus { get; set; }

        public string? Notes { get; set; }
    }



}