namespace whm.DTOs.Unit
{
    // =====================================================
    // UNIT DTO
    // =====================================================

    public class UnitDto
    {
        public int UnitId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Abbreviation { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }


    // =====================================================
    // CREATE UNIT DTO
    // =====================================================

    public class CreateUnitDto
    {
        public string Name { get; set; } = string.Empty;

        public string Abbreviation { get; set; } = string.Empty;
    }


    // =====================================================
    // UPDATE UNIT DTO
    // =====================================================

    public class UpdateUnitDto
    {
        public string Name { get; set; } = string.Empty;

        public string Abbreviation { get; set; } = string.Empty;
    }
}