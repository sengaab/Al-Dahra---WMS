using Microsoft.EntityFrameworkCore;
using whm.Data;
using whm.DTOs.PurchaseOrder;
using whm.Models;

namespace whm.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly DataBaseContext _context;

        public PurchaseOrderRepository(DataBaseContext context)
        {
            _context = context;
        }

        // =========================================================
        // GET ALL
        // =========================================================

        public async Task<IEnumerable<PurchaseOrderDto>> GetAllAsync(
            string? search = null,
            string? status = null,
            int? supplierId = null,
            int? siteId = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _context.PurchaseOrders
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.PONumber.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<PurchaseOrderStatus>(
                    status,
                    true,
                    out var parsedStatus))
                {
                    query = query.Where(x =>
                        x.purchaseOrderStatus == parsedStatus);
                }
            }

            if (supplierId.HasValue)
            {
                query = query.Where(x =>
                    x.SupplierId == supplierId.Value);
            }

            if (siteId.HasValue)
            {
                query = query.Where(x =>
                    x.SiteId == siteId.Value);
            }

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PurchaseOrderDto
                {
                    PurchaseOrderId = x.PurchaseOrderId,
                    PONumber = x.PONumber,

                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier != null
                        ? x.Supplier.Name
                        : string.Empty,

                    SiteId = x.SiteId,
                    SiteName = x.Site != null
                        ? x.Site.Name
                        : string.Empty,

                    OrderDate = x.OrderDate,
                    ExpectedDate = x.ExpectedDate,

                    Status = x.purchaseOrderStatus.ToString(),

                    TotalValue = x.TotalValue,

                    CreatedBy = x.CreatedBy,
                    CreatorName = x.Creator != null
                        ? x.Creator.Name
                        : null,

                    ApprovedBy = x.ApprovedBy,
                    ApproverName = x.Approver != null
                        ? x.Approver.Name
                        : null,

                    ApprovedAt = x.ApprovedAt,

                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                    ItemsCount = x.Items.Count(),

                    TotalOrderedQuantity =
                        x.Items.Sum(i => i.OrderedQuantity),

                    TotalReceivedQuantity =
                        x.Items.Sum(i => i.ReceivedQuantity),

                    TotalRemainingQuantity =
                        x.Items.Sum(i => i.RemainingQuantity)
                })
                .ToListAsync();
        }

        // =========================================================
        // GET BY ID
        // =========================================================

        public async Task<PurchaseOrderDto?> GetByIdAsync(int id)
        {
            return await _context.PurchaseOrders
                .AsNoTracking()
                .Where(x => x.PurchaseOrderId == id)
                .Select(x => new PurchaseOrderDto
                {
                    PurchaseOrderId = x.PurchaseOrderId,
                    PONumber = x.PONumber,

                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier != null
                        ? x.Supplier.Name
                        : string.Empty,

                    SiteId = x.SiteId,
                    SiteName = x.Site != null
                        ? x.Site.Name
                        : string.Empty,

                    OrderDate = x.OrderDate,
                    ExpectedDate = x.ExpectedDate,

                    Status = x.purchaseOrderStatus.ToString(),

                    TotalValue = x.TotalValue,

                    CreatedBy = x.CreatedBy,
                    CreatorName = x.Creator != null
                        ? x.Creator.Name
                        : null,

                    ApprovedBy = x.ApprovedBy,
                    ApproverName = x.Approver != null
                        ? x.Approver.Name
                        : null,

                    ApprovedAt = x.ApprovedAt,

                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                    ItemsCount = x.Items.Count(),

                    TotalOrderedQuantity =
                        x.Items.Sum(i => i.OrderedQuantity),

                    TotalReceivedQuantity =
                        x.Items.Sum(i => i.ReceivedQuantity),

                    TotalRemainingQuantity =
                        x.Items.Sum(i => i.RemainingQuantity)
                })
                .FirstOrDefaultAsync();
        }

        // =========================================================
        // GET ENTITY
        // =========================================================

        public async Task<PurchaseOrder?> GetEntityByIdAsync(int id)
        {
            return await _context.PurchaseOrders
                .FirstOrDefaultAsync(x =>
                    x.PurchaseOrderId == id);
        }

        // =========================================================
        // PO NUMBER EXISTS
        // =========================================================

        public async Task<bool> PONumberExistsAsync(
            string poNumber,
            int? excludeId = null)
        {
            var query = _context.PurchaseOrders
                .AsNoTracking()
                .Where(x => x.PONumber == poNumber);

            if (excludeId.HasValue)
            {
                query = query.Where(x =>
                    x.PurchaseOrderId != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        // =========================================================
        // ITEMS
        // =========================================================

        public async Task<IEnumerable<PurchaseOrderItemDto>> GetItemsAsync(
            int purchaseOrderId)
        {
            return await _context.PurchaseOrderItems
                .AsNoTracking()
                .Where(x =>
                    x.PurchaseOrderId == purchaseOrderId)
                .OrderBy(x => x.PurchaseOrderItemId)
                .Select(x => new PurchaseOrderItemDto
                {
                    PurchaseOrderItemId =
                        x.PurchaseOrderItemId,

                    PurchaseOrderId =
                        x.PurchaseOrderId,

                    ProductId =
                        x.ProductId,

                    ProductName =
                        x.Product != null
                            ? x.Product.Name
                            : string.Empty,

                    SKU =
                        x.Product != null
                            ? x.Product.SKU
                            : string.Empty,

                    OrderedQuantity =
                        x.OrderedQuantity,

                    ReceivedQuantity =
                        x.ReceivedQuantity,

                    RemainingQuantity =
                        x.RemainingQuantity,

                    UnitPrice =
                        x.UnitPrice,

                    TotalPrice =
                        x.TotalPrice
                })
                .ToListAsync();
        }

        public async Task<PurchaseOrderItem?> GetItemEntityByIdAsync(
            int purchaseOrderItemId)
        {
            return await _context.PurchaseOrderItems
                .FirstOrDefaultAsync(x =>
                    x.PurchaseOrderItemId ==
                    purchaseOrderItemId);
        }

        public async Task<PurchaseOrderItemDto?> GetItemByIdAsync(
            int purchaseOrderItemId)
        {
            return await _context.PurchaseOrderItems
                .AsNoTracking()
                .Where(x =>
                    x.PurchaseOrderItemId ==
                    purchaseOrderItemId)
                .Select(x => new PurchaseOrderItemDto
                {
                    PurchaseOrderItemId =
                        x.PurchaseOrderItemId,

                    PurchaseOrderId =
                        x.PurchaseOrderId,

                    ProductId =
                        x.ProductId,

                    ProductName =
                        x.Product != null
                            ? x.Product.Name
                            : string.Empty,

                    SKU =
                        x.Product != null
                            ? x.Product.SKU
                            : string.Empty,

                    OrderedQuantity =
                        x.OrderedQuantity,

                    ReceivedQuantity =
                        x.ReceivedQuantity,

                    RemainingQuantity =
                        x.RemainingQuantity,

                    UnitPrice =
                        x.UnitPrice,

                    TotalPrice =
                        x.TotalPrice
                })
                .FirstOrDefaultAsync();
        }

        // =========================================================
        // RECEIPTS
        // =========================================================

        public async Task<IEnumerable<PurchaseOrderReceiptDto>> GetReceiptsAsync(
            int purchaseOrderId)
        {
            return await _context.Receipts
                .AsNoTracking()
                .Where(x =>
                    x.PurchaseOrderId == purchaseOrderId)
                .OrderByDescending(x => x.ReceivedAt)
                .Select(x => new PurchaseOrderReceiptDto
                {
                    ReceiptId = x.ReceiptId,

                    ReceiptNumber =
                        x.ReceiptNumber,

                    PurchaseOrderId =
                        x.PurchaseOrderId,

                    WarehouseId =
                        x.WarehouseId,

                    WarehouseName =
                        x.Warehouse != null
                            ? x.Warehouse.Name
                            : string.Empty,

                    ReceivedBy =
                        x.ReceivedBy,

                    ReceiverName =
                        x.Receiver != null
                            ? x.Receiver.Name
                            : null,

                    ReceivedAt =
                        x.ReceivedAt,

                    Notes =
                        x.Notes,

                    Status =
                        x.receiptStatus.ToString(),

                    ItemsCount =
                        x.Items.Count()
                })
                .ToListAsync();
        }

        // =========================================================
        // HISTORY
        // =========================================================

        public async Task<IEnumerable<PurchaseOrderHistoryDto>> GetHistoryAsync(
            int purchaseOrderId)
        {
            var result = new List<PurchaseOrderHistoryDto>();

            var order = await _context.PurchaseOrders
                .AsNoTracking()
                .Where(x =>
                    x.PurchaseOrderId == purchaseOrderId)
                .Select(x => new
                {
                    x.PurchaseOrderId,
                    x.CreatedAt,
                    x.CreatedBy,
                    x.purchaseOrderStatus,
                    CreatorName = x.Creator != null
                        ? x.Creator.Name
                        : null,
                    ApproverName = x.Approver != null
                        ? x.Approver.Name
                        : null,
                    x.ApprovedBy,
                    x.ApprovedAt
                })
                .FirstOrDefaultAsync();

            if (order == null)
                return result;

            result.Add(new PurchaseOrderHistoryDto
            {
                EventType = "PurchaseOrderCreated",
                Description = "Purchase order created",
                Status = PurchaseOrderStatus.Draft.ToString(),
                Date = order.CreatedAt,
                UserId = order.CreatedBy,
                UserName = order.CreatorName
            });

            if (order.ApprovedAt.HasValue &&
                order.ApprovedBy.HasValue)
            {
                result.Add(new PurchaseOrderHistoryDto
                {
                    EventType = "PurchaseOrderApproved",
                    Description = "Purchase order approved",
                    Status = PurchaseOrderStatus.Approved.ToString(),
                    Date = order.ApprovedAt.Value,
                    UserId = order.ApprovedBy.Value,
                    UserName = order.ApproverName
                });
            }

            var receipts = await _context.Receipts
                .AsNoTracking()
                .Where(x =>
                    x.PurchaseOrderId == purchaseOrderId)
                .Select(x => new
                {
                    x.ReceiptId,
                    x.ReceiptNumber,
                    x.ReceivedAt,
                    x.ReceivedBy,
                    x.receiptStatus,
                    ReceiverName = x.Receiver != null
                        ? x.Receiver.Name
                        : null
                })
                .ToListAsync();

            foreach (var receipt in receipts)
            {
                result.Add(new PurchaseOrderHistoryDto
                {
                    EventType = "Receipt",
                    Description =
                        $"Receipt {receipt.ReceiptNumber} received",

                    Status =
                        receipt.receiptStatus.ToString(),

                    Date =
                        receipt.ReceivedAt,

                    UserId =
                        receipt.ReceivedBy,

                    UserName =
                        receipt.ReceiverName,

                    ReceiptId =
                        receipt.ReceiptId
                });
            }

            var inspections = await _context.Inspections
                .AsNoTracking()
                .Where(x =>
                    x.ReceiptItem.PurchaseOrderItem.PurchaseOrderId ==
                    purchaseOrderId)
                .Select(x => new
                {
                    x.InspectionId,
                    x.ReceiptItemId,
                    x.InspectedAt,
                    x.InspectedBy,
                    x.InspectionStatus,
                    InspectorName = x.Inspector != null
                        ? x.Inspector.Name
                        : null
                })
                .ToListAsync();

            foreach (var inspection in inspections)
            {
                result.Add(new PurchaseOrderHistoryDto
                {
                    EventType = "Inspection",
                    Description =
                        "Receipt item inspection",

                    Status =
                        inspection.InspectionStatus.ToString(),

                    Date =
                        inspection.InspectedAt,

                    UserId =
                        inspection.InspectedBy,

                    UserName =
                        inspection.InspectorName,

                    ReceiptItemId =
                        inspection.ReceiptItemId,

                    InspectionId =
                        inspection.InspectionId
                });
            }

            return result
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        // =========================================================
        // ADD / UPDATE / DELETE
        // =========================================================

        public async Task AddAsync(PurchaseOrder purchaseOrder)
        {
            await _context.PurchaseOrders.AddAsync(purchaseOrder);
        }

        public void Update(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Update(purchaseOrder);
        }

        public void Delete(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Remove(purchaseOrder);
        }

        // =========================================================
        // ITEM ADD / UPDATE / DELETE
        // =========================================================

        public async Task AddItemAsync(PurchaseOrderItem item)
        {
            await _context.PurchaseOrderItems.AddAsync(item);
        }

        public void UpdateItem(PurchaseOrderItem item)
        {
            _context.PurchaseOrderItems.Update(item);
        }

        public void DeleteItem(PurchaseOrderItem item)
        {
            _context.PurchaseOrderItems.Remove(item);
        }
    }
}