using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET DASHBOARD
        // GET: api/Dashboard
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            // =================================================
            // DATE
            // =================================================

            var today = DateTimeOffset.UtcNow.Date;

            var tomorrow = today.AddDays(1);

            var endOfWeek = today.AddDays(7);


            // =================================================
            // PRODUCTS
            // =================================================

            var productsQuery =
                unitOfWork.Dashboard
                    .GetProductsQuery();


            // =================================================
            // STOCK
            // =================================================

            var stocksQuery =
                unitOfWork.Dashboard
                    .GetStocksQuery()

                    .Include(s => s.Product)
                        .ThenInclude(p => p.Category)

                    .Include(s => s.Bin)
                        .ThenInclude(b => b.Shelf)
                            .ThenInclude(sh => sh.Row)
                                .ThenInclude(r => r.Room)
                                    .ThenInclude(room => room.Warehouse);


            // =================================================
            // PRODUCT ITEMS
            // ProductItem → Stock → Bin
            // =================================================

            var productItemsQuery =
                unitOfWork.Dashboard
                    .GetProductItemsQuery()

                    .Include(i => i.Product)

                    .Include(i => i.Stock)
                        .ThenInclude(s => s.Bin);


            // =================================================
            // WAREHOUSES
            // =================================================

            var warehousesQuery =
                unitOfWork.Dashboard
                    .GetWarehousesQuery()

                    .Include(w => w.Site);


            // =================================================
            // ORDERS
            // =================================================

            var ordersQuery =
                unitOfWork.Dashboard
                    .GetOrdersQuery()

                    .Include(o => o.Supplier)

                    .Include(o => o.Warehouse)

                    .Include(o => o.OrderItems);


            // =================================================
            // 1. DASHBOARD STATS
            // =================================================

            var totalSkus =
                await productsQuery
                    .Select(p => p.SKU)
                    .Distinct()
                    .CountAsync();


            var stockUnits =
                await stocksQuery
                    .SumAsync(s => s.Quantity);


            var stockValue =
                await stocksQuery
                    .SumAsync(s =>
                        s.Quantity * s.UnitPrice);


            var lowStock =
                await stocksQuery
                    .CountAsync(s =>
                        s.Quantity <= s.MinimumStock);


            var stats =
                new DashboardStatsDto
                {
                    TotalSkus =
                        totalSkus,

                    StockUnits =
                        stockUnits,

                    StockValue =
                        stockValue,

                    LowStock =
                        lowStock
                };


            // =================================================
            // 2. VALUE BY CATEGORY
            // =================================================

            var valueByCategory =
                await stocksQuery

                    .Where(s =>
                        s.Product != null &&
                        s.Product.Category != null)

                    .GroupBy(s => new
                    {
                        CategoryId =
                            s.Product.CategoryId,

                        CategoryName =
                            s.Product.Category.Category_Name
                    })

                    .Select(g => new CategoryValueDto
                    {
                        CategoryId =
                            g.Key.CategoryId,

                        CategoryName =
                            g.Key.CategoryName,

                        Value =
                            g.Sum(s =>
                                s.Quantity *
                                s.UnitPrice)
                    })

                    .OrderByDescending(x => x.Value)

                    .ToListAsync();


            // =================================================
            // 3. WAREHOUSE OVERVIEW
            // =================================================

            var warehouseOverview =
                await warehousesQuery

                    .Select(w => new WarehouseOverviewDto
                    {
                        WarehouseId =
                            w.Warehouse_Id,

                        WarehouseName =
                            w.Warehouse_Name,

                        SiteName =
                            w.Site != null
                                ? w.Site.Site_Name
                                : string.Empty,

                        Skus =
                            stocksQuery
                                .Count(s =>
                                    s.Bin != null &&
                                    s.Bin.Shelf != null &&
                                    s.Bin.Shelf.Row != null &&
                                    s.Bin.Shelf.Row.Room != null &&
                                    s.Bin.Shelf.Row.Room.Warehouse_Id
                                        == w.Warehouse_Id),

                        Units =
                            stocksQuery
                                .Where(s =>
                                    s.Bin != null &&
                                    s.Bin.Shelf != null &&
                                    s.Bin.Shelf.Row != null &&
                                    s.Bin.Shelf.Row.Room != null &&
                                    s.Bin.Shelf.Row.Room.Warehouse_Id
                                        == w.Warehouse_Id)

                                .Sum(s => s.Quantity),

                        Occupancy =
                            0,

                        StockStatus =
                            "Good"
                    })

                    .ToListAsync();


            // =================================================
            // 4. STOCK STATUS
            // =================================================

            var totalStockUnits =
                await stocksQuery
                    .SumAsync(s => s.Quantity);


            var stockStatus =
                await stocksQuery

                    .GroupBy(s => s.StockStatus)

                    .Select(g => new StockStatusDto
                    {
                        Status =
                            g.Key.ToString(),

                        Quantity =
                            g.Sum(s =>
                                s.Quantity),

                        Percentage =
                            totalStockUnits == 0
                                ? 0
                                : Math.Round(
                                    (g.Sum(s => s.Quantity) /
                                    totalStockUnits) * 100,
                                    2)
                    })

                    .ToListAsync();


            // =================================================
            // 5. INCOMING STOCK
            // =================================================

            var expectedToday =
                await ordersQuery

                    .Where(o =>
                        o.ExpectedDate.HasValue &&

                        o.ExpectedDate.Value.Date == today &&

                        o.Status != OrderStatus.Cancelled &&

                        o.Status != OrderStatus.Received)

                    .CountAsync();


            var expectedThisWeek =
                await ordersQuery

                    .Where(o =>
                        o.ExpectedDate.HasValue &&

                        o.ExpectedDate.Value.Date >= today &&

                        o.ExpectedDate.Value.Date < endOfWeek &&

                        o.Status != OrderStatus.Cancelled &&

                        o.Status != OrderStatus.Received)

                    .CountAsync();


            var pendingReceiving =
                await ordersQuery

                    .CountAsync(o =>
                        o.Status == OrderStatus.Pending ||

                        o.Status == OrderStatus.Approved ||

                        o.Status == OrderStatus.Ordered ||

                        o.Status == OrderStatus.PartiallyReceived);


            var inTransit =
                await ordersQuery

                    .CountAsync(o =>
                        o.Status == OrderStatus.Ordered);


            // =================================================
            // INCOMING ORDERS
            // =================================================

            var incomingOrders =
                await ordersQuery

                    .Where(o =>
                        o.Status != OrderStatus.Cancelled &&

                        o.Status != OrderStatus.Received)

                    .OrderBy(o => o.ExpectedDate)

                    .Take(20)

                    .Select(o => new IncomingStockOrderDto
                    {
                        PORef =
                            o.OrderNumber,

                        Supplier =
                            o.Supplier != null
                                ? o.Supplier.SupplierName
                                : string.Empty,

                        Units =
                            o.OrderItems
                                .Sum(i => i.Quantity),

                        ExpectedDate =
                            o.ExpectedDate.HasValue
                                ? o.ExpectedDate.Value.DateTime
                                : null,

                        Status =
                            o.Status.ToString()
                    })

                    .ToListAsync();


            var incomingStock =
                new IncomingStockDto
                {
                    ExpectedToday =
                        expectedToday,

                    ExpectedThisWeek =
                        expectedThisWeek,

                    PendingReceiving =
                        pendingReceiving,

                    InTransit =
                        inTransit,

                    Orders =
                        incomingOrders
                };


            // =================================================
            // 6. RECENT ACTIVITY
            // =================================================

            var recentActivity =
                await unitOfWork.Dashboard
                    .GetTransactionsQuery()

                    .Include(o => o.Product)

                    .Include(o => o.User)

                    .Include(o => o.FromBin)

                    .Include(o => o.ToBin)

                    .OrderByDescending(o => o.CreateAt)

                    .Take(20)

                    .Select(o => new RecentActivityDto
                    {
                        Time =
                            o.CreateAt,

                        User =
                            o.User != null
                                ? o.User.User_Name
                                : "Unknown",

                        Action =
                            o.OperationType.ToString(),

                        Description =
                            o.Notes,

                        From =
                            o.FromBin != null
                                ? o.FromBin.Bin_Name
                                : null,

                        To =
                            o.ToBin != null
                                ? o.ToBin.Bin_Name
                                : null
                    })

                    .ToListAsync();


            // =================================================
            // 7. STOCKS
            // ProductId + StockId + SKU
            // =================================================

            var dashboardStocks =
                await stocksQuery

                    .Select(s => new DashboardStockDto
                    {
                        ProductId =
                            s.ProductId,

                        StockId =
                            s.Stock_Id,

                        SKU =
                            s.Product != null
                                ? s.Product.SKU
                                : string.Empty,

                        ProductName =
                            s.Product != null
                                ? s.Product.ProductName
                                : string.Empty,

                        Quantity =
                            s.Quantity
                    })

                    .ToListAsync();


            // =================================================
            // 8. PRODUCT ITEMS
            // ProductItem → Product
            // ProductItem → Stock → Bin
            // =================================================

            var dashboardProductItems =
                await productItemsQuery

                    .Select(i => new DashboardProductItemDto
                    {
                        // =========================
                        // Product Item
                        // =========================

                        ItemId =
                            i.ItemId,

                        ItemCode =
                            i.ItemCode,

                        Barcode =
                            i.Barcode,

                        QRValue =
                            i.QRValue,


                        // =========================
                        // Product
                        // =========================

                        ProductId =
                            i.ProductId,

                        SKU =
                            i.Product != null
                                ? i.Product.SKU
                                : string.Empty,

                        ProductName =
                            i.Product != null
                                ? i.Product.ProductName
                                : string.Empty,


                        // =========================
                        // Stock
                        // =========================

                        StockId =
                            i.StockId,

                        StockCode =
                            i.Stock != null
                                ? i.Stock.StockCode
                                : string.Empty,


                        // =========================
                        // Location
                        // =========================

                        BinId =
                            i.Stock != null
                                ? i.Stock.Bin_Id
                                : null,

                        BinName =
                            i.Stock != null &&
                            i.Stock.Bin != null
                                ? i.Stock.Bin.Bin_Name
                                : null,


                        // =========================
                        // Status
                        // =========================

                        IsActive =
                            i.IsActive
                    })

                    .ToListAsync();


            // =================================================
            // FINAL RESPONSE
            // =================================================

            var dashboard =
                new DashboardResponseDto
                {
                    Stats =
                        stats,

                    ValueByCategory =
                        valueByCategory,

                    WarehouseOverview =
                        warehouseOverview,

                    StockStatus =
                        stockStatus,

                    IncomingStock =
                        incomingStock,

                    RecentActivity =
                        recentActivity,

                    Stocks =
                        dashboardStocks,

                    ProductItems =
                        dashboardProductItems
                };


            return Ok(dashboard);
        }
    }
}