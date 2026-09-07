using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly DataBaseContext _context;

        public InspectionRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inspection>> GetAllAsync()
        {
            return await _context.Inspections
                .Include(i => i.ReceiptItem)
                .Include(i => i.Inspector)
                .AsNoTracking()
                .OrderByDescending(i => i.InspectedAt)
                .ToListAsync();
        }

        public async Task<Inspection?> GetByIdAsync(int id)
        {
            return await _context.Inspections
                .Include(i => i.ReceiptItem)
                .Include(i => i.Inspector)
                .FirstOrDefaultAsync(i => i.InspectionId == id);
        }

        public async Task<Inspection?> GetByReceiptItemIdAsync(int receiptItemId)
        {
            return await _context.Inspections
                .FirstOrDefaultAsync(i => i.ReceiptItemId == receiptItemId);
        }

        public async Task<Inspection> AddAsync(Inspection inspection)
        {
            await _context.Inspections.AddAsync(inspection);

            return inspection;
        }

        public Task UpdateAsync(Inspection inspection)
        {
            _context.Inspections.Update(inspection);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Inspections
                .AnyAsync(i => i.InspectionId == id);
        }
    }
}