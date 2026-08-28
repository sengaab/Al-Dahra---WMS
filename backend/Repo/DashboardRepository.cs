using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.Dashboard;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataBaseContext _context;

        public DashboardRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardAsync(
            int? siteId,
            int? departmentId,
            int? warehouseId,
            DateTimeOffset? fromDate,
            DateTimeOffset? toDate)
        {
            var stocks = _context.Stocks
                .AsNoTracking()
                .AsQueryable();

            var warehouses = _context.Warehouses
                .AsNoTracking()
                .AsQueryable();

            // =====================================================
            // WAREHOUSE FILTER
            // =====================================================

            if (warehouseId.HasValue)
            {
                stocks = stocks.Where(x =>
                    x.WarehouseId == warehouseId.Value);

                warehouses = warehouses.Where(x =>
                    x.WarehouseId == warehouseId.Value);
            }


            // =====================================================
            // SITE FILTER
            // =====================================================

            if (siteId.HasValue)
            {
                stocks = stocks.Where(x =>
                    x.Warehouse.SiteId == siteId.Value);

                warehouses = warehouses.Where(x =>
                    x.SiteId == siteId.Value);
            }


            // =====================================================
            // DATE FILTER
            // =====================================================

            if (fromDate.HasValue)
            {
                stocks = stocks.Where(x =>
                    x.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                stocks = stocks.Where(x =>
                    x.CreatedAt <= toDate.Value);
            }


            // =====================================================
            // STATS
            // =====================================================

            var stats = new DashboardStatsDto
            {
                TotalProducts = await stocks
                    .Select(x => x.ProductId)
                    .Distinct()
                    .CountAsync(),

                TotalStockItems = await stocks.CountAsync(),

                TotalQuantity = await stocks
                    .SumAsync(x => (decimal?)x.Quantity) ?? 0,

                TotalStockValue = await stocks
                    .SumAsync(x => (decimal?)(x.Quantity * x.UnitPrice)) ?? 0,

                LowStockItems = await stocks
                    .CountAsync(x =>
                        x.AvailableQuantity > 0 &&
                        x.AvailableQuantity <= x.MinimumStock),

                OutOfStockItems = await stocks
                    .CountAsync(x =>
                        x.AvailableQuantity <= 0),

                ActiveWarehouses = await warehouses
                    .CountAsync(x => x.IsActive)
            };


            // =====================================================
            // WAREHOUSE OVERVIEW
            // =====================================================

            var warehouseOverview = await stocks
                .GroupBy(x => new
                {
                    x.WarehouseId,
                    WarehouseName = x.Warehouse.Name
                })
                .Select(g => new WarehouseOverviewDto
                {
                    WarehouseId = g.Key.WarehouseId,

                    WarehouseName = g.Key.WarehouseName,

                    StockItems = g.Count(),

                    Quantity = g.Sum(x => x.Quantity),

                    TotalValue = g.Sum(x =>
                        x.Quantity * x.UnitPrice)
                })
                .ToListAsync();


            // =====================================================
            // STOCK STATUS
            // =====================================================

            var stockStatus = await stocks
                .GroupBy(x => x.stockStatus)
                .Select(g => new StockStatusDto
                {
                    Status = g.Key.ToString(),

                    Count = g.Count(),

                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToListAsync();


            // =====================================================
            // LOW STOCK
            // =====================================================

            var lowStock = await stocks
                .Where(x =>
                    x.AvailableQuantity > 0 &&
                    x.AvailableQuantity <= x.MinimumStock)
                .OrderBy(x => x.AvailableQuantity)
                .Select(x => new LowStockDto
                {
                    StockId = x.StockId,

                    ProductId = x.ProductId,

                    ProductName = x.Product.Name,

                    SKU = x.Product.SKU,

                    WarehouseId = x.WarehouseId,

                    WarehouseName = x.Warehouse.Name,

                    Quantity = x.Quantity,

                    AvailableQuantity = x.AvailableQuantity,

                    MinimumStock = x.MinimumStock
                })
                .Take(20)
                .ToListAsync();


            // =====================================================
            // VALUE BY CATEGORY
            // =====================================================

            var valueByCategory = await stocks
                .GroupBy(x => new
                {
                    CategoryId = x.Product.CategoryId,

                    CategoryName = x.Product.Category != null
                        ? x.Product.Category.Name
                        : "Uncategorized"
                })
                .Select(g => new ValueByCategoryDto
                {
                    CategoryId = g.Key.CategoryId,

                    CategoryName = g.Key.CategoryName,

                    TotalValue = g.Sum(x =>
                        x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.TotalValue)
                .ToListAsync();


            // =====================================================
            // RECENT TRANSACTIONS
            // =====================================================

            var transactions = _context.StockTransactions
                .AsNoTracking()
                .AsQueryable();

            if (warehouseId.HasValue)
            {
                transactions = transactions.Where(x =>
                    x.Stock.WarehouseId == warehouseId.Value);
            }

            if (siteId.HasValue)
            {
                transactions = transactions.Where(x =>
                    x.Stock.Warehouse.SiteId == siteId.Value);
            }

            if (fromDate.HasValue)
            {
                transactions = transactions.Where(x =>
                    x.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                transactions = transactions.Where(x =>
                    x.CreatedAt <= toDate.Value);
            }

            var recentTransactions = await transactions
                .OrderByDescending(x => x.CreatedAt)
                .Take(20)
                .Select(x => new RecentTransactionDto
                {
                    TransactionId = x.TransactionId,

                    ProductId = x.ProductId,

                    ProductName = x.Product.Name,

                    TransactionType = x.TransactionType,

                    Quantity = x.Quantity,

                    ReferenceType = x.ReferenceType,

                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();


            // =====================================================
            // PENDING RECEIPTS
            // =====================================================

            var receipts = _context.Receipts
                .AsNoTracking()
                .AsQueryable();

            if (warehouseId.HasValue)
            {
                receipts = receipts.Where(x =>
                    x.WarehouseId == warehouseId.Value);
            }

            if (siteId.HasValue)
            {
                receipts = receipts.Where(x =>
                    x.Warehouse.SiteId == siteId.Value);
            }

            var pendingReceipts = await receipts
                .Where(x =>
                    x.receiptStatus == ReceiptStatus.Pending ||
                    x.receiptStatus == ReceiptStatus.InProgress ||
                    x.receiptStatus == ReceiptStatus.PendingInspection)
                .OrderByDescending(x => x.ReceivedAt)
                .Take(10)
                .Select(x => new PendingItemDto
                {
                    Id = x.ReceiptId,

                    Number = x.ReceiptNumber,

                    Status = x.receiptStatus.ToString(),

                    CreatedAt = x.ReceivedAt
                })
                .ToListAsync();


            // =====================================================
            // PENDING REQUESTS
            // =====================================================

            var requests = _context.StockRequests
                .AsNoTracking()
                .AsQueryable();

            if (departmentId.HasValue)
            {
                requests = requests.Where(x =>
                    x.DepartmentId == departmentId.Value);
            }

            if (siteId.HasValue)
            {
                requests = requests.Where(x =>
                    x.SiteId == siteId.Value);
            }

            var pendingRequests = await requests
                .Where(x =>
                    x.StockRequestStatus == StockRequestStatus.Submitted ||
                    x.StockRequestStatus == StockRequestStatus.PendingApproval ||
                    x.StockRequestStatus == StockRequestStatus.Approved ||
                    x.StockRequestStatus == StockRequestStatus.PartiallyReserved)
                .OrderByDescending(x => x.RequestedAt)
                .Take(10)
                .Select(x => new PendingItemDto
                {
                    Id = x.RequestId,

                    Number = x.RequestNumber,

                    Status = x.StockRequestStatus.ToString(),

                    CreatedAt = x.RequestedAt
                })
                .ToListAsync();


            // =====================================================
            // PENDING PICK LISTS
            // =====================================================

            var pickLists = _context.PickLists
                .AsNoTracking()
                .AsQueryable();

            if (warehouseId.HasValue)
            {
                pickLists = pickLists.Where(x =>
                    x.WarehouseId == warehouseId.Value);
            }

            if (siteId.HasValue)
            {
                pickLists = pickLists.Where(x =>
                    x.Warehouse.SiteId == siteId.Value);
            }

            var pendingPickLists = await pickLists
                .Where(x =>
                    x.PickListStatus == PickListStatus.Pending ||
                    x.PickListStatus == PickListStatus.Assigned ||
                    x.PickListStatus == PickListStatus.InProgress ||
                    x.PickListStatus == PickListStatus.PartiallyPicked)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Select(x => new PendingItemDto
                {
                    Id = x.PickListId,

                    Number = x.PickNumber,

                    Status = x.PickListStatus.ToString(),

                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();


            // =====================================================
            // PENDING TRANSFERS
            // =====================================================

            var transfers = _context.StockTransfers
                .AsNoTracking()
                .AsQueryable();

            if (warehouseId.HasValue)
            {
                transfers = transfers.Where(x =>
                    x.SourceWarehouseId == warehouseId.Value ||
                    x.DestinationWarehouseId == warehouseId.Value);
            }

            if (siteId.HasValue)
            {
                transfers = transfers.Where(x =>
                    x.SourceWarehouse.SiteId == siteId.Value ||
                    x.DestinationWarehouse.SiteId == siteId.Value);
            }

            var pendingTransfers = await transfers
                .Where(x =>
                    x.TransferStatus == StockTransferStatus.Pending ||
                    x.TransferStatus == StockTransferStatus.Approved ||
                    x.TransferStatus == StockTransferStatus.Picking ||
                    x.TransferStatus == StockTransferStatus.ReadyToShip ||
                    x.TransferStatus == StockTransferStatus.InTransit ||
                    x.TransferStatus == StockTransferStatus.PartiallyReceived)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Select(x => new PendingItemDto
                {
                    Id = x.TransferId,

                    Number = x.TransferNumber,

                    Status = x.TransferStatus.ToString(),

                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();


            // =====================================================
            // RETURN DASHBOARD
            // =====================================================

            return new DashboardDto
            {
                Stats = stats,

                WarehouseOverview = warehouseOverview,

                StockStatus = stockStatus,

                LowStock = lowStock,

                ValueByCategory = valueByCategory,

                RecentTransactions = recentTransactions,

                PendingReceipts = pendingReceipts,

                PendingRequests = pendingRequests,

                PendingPickLists = pendingPickLists,

                PendingTransfers = pendingTransfers
            };
        }
    }
}