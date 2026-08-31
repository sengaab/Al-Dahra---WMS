using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Unit;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly DataBaseContext _context;

        public UnitRepository(DataBaseContext context)
        {
            _context = context;
        }


        // =====================================================
        // GET ALL
        // GET /api/units
        // =====================================================

        public async Task<List<UnitDto>> GetAllAsync()
        {
            return await BuildQuery()
                .ToListAsync();
        }


        // =====================================================
        // GET BY ID
        // GET /api/units/{id}
        // =====================================================

        public async Task<UnitDto?> GetByIdAsync(int id)
        {
            return await BuildQuery()
                .FirstOrDefaultAsync(x => x.UnitId == id);
        }


        // =====================================================
        // GET ENTITY
        // =====================================================

        public async Task<Unit?> GetEntityByIdAsync(int id)
        {
            return await _context.Units
                .FirstOrDefaultAsync(x => x.UnitId == id);
        }


        // =====================================================
        // ADD
        // =====================================================

        public async Task AddAsync(Unit unit)
        {
            await _context.Units.AddAsync(unit);
        }


        // =====================================================
        // UPDATE
        // =====================================================

        public void Update(Unit unit)
        {
            _context.Units.Update(unit);
        }


        // =====================================================
        // DELETE
        // =====================================================

        public void Delete(Unit unit)
        {
            _context.Units.Remove(unit);
        }


        // =====================================================
        // COMMON QUERY
        // =====================================================

        private IQueryable<UnitDto> BuildQuery()
        {
            return _context.Units
                .AsNoTracking()
                .Select(x => new UnitDto
                {
                    UnitId = x.UnitId,

                    Name = x.Name,

                    Abbreviation = x.Abbreviation,

                    CreatedAt = x.CreatedAt,

                    UpdatedAt = x.UpdatedAt
                });
        }
    }
}