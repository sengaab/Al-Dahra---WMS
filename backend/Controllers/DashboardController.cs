using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whm.Models;
using whm.DTOs;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public DashboardController(DataBaseContext context)
        {
            _context = context;
        }

        // GET:
        // /api/dashboard
        //
        // Optional filters:
        // /api/dashboard?siteId=1
        // /api/dashboard?departmentId=2
        // /api/dashboard?siteId=1&departmentId=2

        [HttpGet]
        public async Task<ActionResult<DashboardResponse>> GetDashboard(
            [FromQuery] int? siteId = null,
            [FromQuery] int? departmentId = null)
        {
            // =====================================================
            // Base product query
            // =====================================================

            var productsQuery = _context.Products
                .AsNoTracking()
                .AsQueryable();

            if (departmentId.HasValue)
            {
                productsQuery = productsQuery.Where(
                    p => p.Category.Department_Id == departmentId.Value
                );
            }

            if (siteId.HasValue)
            {
                productsQuery = productsQuery.Where(
                    p => p.Stock.Any(
                        s => s.IsActive &&
                             s.Bin.Shelf.Row.Room.Warehouse.Site_Id == siteId.Value
                    )
                );
            }


            // =====================================================
            // Base stock query
            // =====================================================

            var stockQuery = _context.Stocks
                .AsNoTracking()
                .Where(s => s.IsActive)
                .AsQueryable();


            // Filter by department

            if (departmentId.HasValue)
            {
                stockQuery = stockQuery.Where(
                    s => s.Product.Category.Department_Id == departmentId.Value
                );
            }


            // Filter by site

            if (siteId.HasValue)
            {
                stockQuery = stockQuery.Where(
                    s => s.Bin.Shelf.Row.Room.Warehouse.Site_Id == siteId.Value
                );
            }


            // =====================================================
            // 1. TOTAL SKUS
            // =====================================================

            var totalSkus = await productsQuery
                .CountAsync();


            // =====================================================
            // 2. STOCK UNITS
            // =====================================================

            var stockUnits = await stockQuery
                .SumAsync(s => (decimal?)s.Quantity) ?? 0;


            // =====================================================
            // 3. STOCK VALUE
            // Quantity × UnitPrice
            // =====================================================

            var stockValue = await stockQuery
                .SumAsync(
                    s => (decimal?)(s.Quantity * s.Product.UnitPrice)
                ) ?? 0;


            // =====================================================
            // 4. LOW STOCK
            //
            // Product is low stock when total quantity <=
            // MinimumStock
            // =====================================================

            var lowStockProducts = await productsQuery
                .Select(p => new
                {
                    p.ProductId,
                    p.MinimumStock,

                    Quantity = p.Stock
                        .Where(s => s.IsActive)
                        .Where(s =>
                            !siteId.HasValue ||
                            s.Bin.Shelf.Row.Room.Warehouse.Site_Id == siteId.Value
                        )
                        .Sum(s => (decimal?)s.Quantity) ?? 0
                })
                .Where(x => x.Quantity <= x.MinimumStock)
                .CountAsync();


            // =====================================================
            // 5. VALUE BY CATEGORY
            // =====================================================

            var valueByCategory = await stockQuery
                .GroupBy(s => new
                {
                    s.Product.Category.Category_Id,
                    s.Product.Category.Category_Name
                })
                .Select(g => new CategoryValueDto
                {
                    CategoryId = g.Key.Category_Id,

                    CategoryName = g.Key.Category_Name,

                    Value = g.Sum(
                        s => s.Quantity * s.Product.UnitPrice
                    )
                })
                .OrderByDescending(x => x.Value)
                .ToListAsync();


            // =====================================================
            // 6. WAREHOUSE OVERVIEW
            // =====================================================

            var warehouseQuery = _context.Warehouses
                .AsNoTracking()
                .Where(w => w.IsActive)
                .AsQueryable();


            if (siteId.HasValue)
            {
                warehouseQuery = warehouseQuery.Where(
                    w => w.Site_Id == siteId.Value
                );
            }


            var warehouseOverview = await warehouseQuery
                .Select(w => new WarehouseOverviewDto
                {
                    WarehouseId = w.Warehouse_Id,

                    WarehouseName = w.Warehouse_Name,

                    SiteName = w.Site.Site_Name,

                    Skus = w.Rooms
                        .SelectMany(r => r.Rows)
                        .SelectMany(row => row.Shelves)
                        .SelectMany(shelf => shelf.Bins)
                        .SelectMany(bin => bin.Stocks)
                        .Where(s => s.IsActive)
                        .Where(s =>
                            !departmentId.HasValue ||
                            s.Product.Category.Department_Id == departmentId.Value
                        )
                        .Select(s => s.ProductId)
                        .Distinct()
                        .Count(),

                    Units = w.Rooms
                        .SelectMany(r => r.Rows)
                        .SelectMany(row => row.Shelves)
                        .SelectMany(shelf => shelf.Bins)
                        .SelectMany(bin => bin.Stocks)
                        .Where(s => s.IsActive)
                        .Where(s =>
                            !departmentId.HasValue ||
                            s.Product.Category.Department_Id == departmentId.Value
                        )
                        .Sum(s => (decimal?)s.Quantity) ?? 0,

                    Occupancy =
                        w.Rooms
                        .SelectMany(r => r.Rows)
                        .SelectMany(row => row.Shelves)
                        .SelectMany(shelf => shelf.Bins)
                        .Count() == 0

                        ? 0

                        :

                        (
                            w.Rooms
                            .SelectMany(r => r.Rows)
                            .SelectMany(row => row.Shelves)
                            .SelectMany(shelf => shelf.Bins)
                            .Count(b =>
                                b.Stocks.Any(s =>
                                    s.IsActive &&
                                    (
                                        !departmentId.HasValue ||
                                        s.Product.Category.Department_Id ==
                                            departmentId.Value
                                    )
                                )
                            ) * 100m
                        )
                        /
                        w.Rooms
                        .SelectMany(r => r.Rows)
                        .SelectMany(row => row.Shelves)
                        .SelectMany(shelf => shelf.Bins)
                        .Count(),

                    StockStatus =
                        w.Rooms
                        .SelectMany(r => r.Rows)
                        .SelectMany(row => row.Shelves)
                        .SelectMany(shelf => shelf.Bins)
                        .SelectMany(bin => bin.Stocks)
                        .Where(s => s.IsActive)
                        .Any(s =>
                            s.Product.Status == ProductStatus.Expired ||
                            s.Product.Status == ProductStatus.Damage
                        )
                        ? "Attention"
                        : "Good"
                })
                .ToListAsync();


            // =====================================================
            // 7. STOCK STATUS
            //
            // Uses Product.Status:
            //
            // Available
            // Reserved
            // Damage
            // Expired
            // Quarantined
            // =====================================================

            var stockStatusRaw = await stockQuery
                .GroupBy(s => s.Product.Status)
                .Select(g => new
                {
                    Status = g.Key,

                    Quantity = g.Sum(s => s.Quantity)
                })
                .ToListAsync();


            var totalStatusQuantity = stockStatusRaw
                .Sum(x => x.Quantity);


            var stockStatus = stockStatusRaw
                .Select(x => new StockStatusDto
                {
                    Status = x.Status.ToString(),

                    Quantity = x.Quantity,

                    Percentage = totalStatusQuantity == 0
                        ? 0
                        : Math.Round(
                            x.Quantity / totalStatusQuantity * 100,
                            1
                        )
                })
                .OrderByDescending(x => x.Quantity)
                .ToList();


            // =====================================================
            // 8. INCOMING STOCK
            //
            // Current schema has:
            //
            // StockStatus:
            // InTransit
            // Pending
            // Received
            //
            // There is currently no PurchaseOrder/Supplier/
            // ExpectedDate table.
            // =====================================================

            var pendingReceiving = await stockQuery
                .Where(s => s.StockStatue == StockStatus.Pending)
                .CountAsync();


            var inTransit = await stockQuery
                .Where(s => s.StockStatue == StockStatus.InTransit)
                .CountAsync();


            var incomingStock = new IncomingStockDto
            {
                // Cannot calculate these accurately because
                // there is no ExpectedDate in the current schema.

                ExpectedToday = 0,

                ExpectedThisWeek = 0,

                PendingReceiving = pendingReceiving,

                InTransit = inTransit,

                Orders = new List<IncomingStockOrderDto>()
            };


            // =====================================================
            // 9. RECENT ACTIVITY
            //
            // Use Transactions because they contain:
            // User
            // Product
            // FromBin
            // ToBin
            // Quantity
            // TransactionType
            // CreateAt
            // =====================================================

            var recentTransactionsQuery = _context.Transactions
                .AsNoTracking()
                .Where(t =>
                    !departmentId.HasValue ||
                    t.Product.Category.Department_Id == departmentId.Value
                )
                .AsQueryable();


            if (siteId.HasValue)
            {
                recentTransactionsQuery =
                    recentTransactionsQuery.Where(t =>
                        (t.FromBin != null &&
                         t.FromBin.Shelf.Row.Room.Warehouse.Site_Id ==
                            siteId.Value)

                        ||

                        (t.ToBin != null &&
                         t.ToBin.Shelf.Row.Room.Warehouse.Site_Id ==
                            siteId.Value)
                    );
            }


            var recentActivity = await recentTransactionsQuery
                .OrderByDescending(t => t.CreateAt)
                .Take(10)
                .Select(t => new RecentActivityDto
                {
                    Time = t.CreateAt,

                    User = t.User.User_Name,

                    Action = t.TransactionType.ToString(),

                    Description =
                        t.Quantity.ToString() +
                        " units of " +
                        t.Product.ProductName,

                    From =
                        t.FromBin != null
                            ? t.FromBin.Shelf.Row.Room.Warehouse.Warehouse_Name
                            : null,

                    To =
                        t.ToBin != null
                            ? t.ToBin.Shelf.Row.Room.Warehouse.Warehouse_Name
                            : null
                })
                .ToListAsync();


            // =====================================================
            // RESPONSE
            // =====================================================

            var response = new DashboardResponse
            {
                Stats = new DashboardStatsDto
                {
                    TotalSkus = totalSkus,

                    StockUnits = stockUnits,

                    StockValue = stockValue,

                    LowStock = lowStockProducts
                },

                ValueByCategory = valueByCategory,

                WarehouseOverview = warehouseOverview,

                StockStatus = stockStatus,

                IncomingStock = incomingStock,

                RecentActivity = recentActivity
            };


            return Ok(response);
        }
    }
}