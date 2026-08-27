using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class StockIssueRepository : IStockIssueRepository
    {
        private readonly DataBaseContext _context;

        public StockIssueRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =========================
        // Get All
        // =========================
        public async Task<List<StockIssue>> GetAllAsync()
        {
            return await _context.StockIssues
                .AsNoTracking()
                .Include(x => x.Items)
                .Include(x => x.StockRequest)
                .Include(x => x.PickList)
                .Include(x => x.Warehouse)
                .Include(x => x.Department)
                .Include(x => x.Issuer)
                .OrderByDescending(x => x.IssueId)
                .ToListAsync();
        }


        // =========================
        // Get By ID
        // =========================
        public async Task<StockIssue?> GetByIdAsync(int issueId)
        {
            return await _context.StockIssues
                .AsNoTracking()
                .Include(x => x.Items)
                .Include(x => x.StockRequest)
                .Include(x => x.PickList)
                .Include(x => x.Warehouse)
                .Include(x => x.Department)
                .Include(x => x.Issuer)
                .FirstOrDefaultAsync(x => x.IssueId == issueId);
        }


        // =========================
        // Get By ID For Update
        // =========================
        public async Task<StockIssue?> GetByIdForUpdateAsync(int issueId)
        {
            return await _context.StockIssues
                .Include(x => x.Items)
                .Include(x => x.StockRequest)
                .Include(x => x.PickList)
                .FirstOrDefaultAsync(x => x.IssueId == issueId);
        }


        // =========================
        // Add
        // =========================
        public async Task AddAsync(StockIssue stockIssue)
        {
            await _context.StockIssues.AddAsync(stockIssue);
        }


        // =========================
        // Update
        // =========================
        public void Update(StockIssue stockIssue)
        {
            _context.StockIssues.Update(stockIssue);
        }


        // =========================
        // Exists
        // =========================
        public async Task<bool> ExistsAsync(int issueId)
        {
            return await _context.StockIssues
                .AnyAsync(x => x.IssueId == issueId);
        }
    }
}