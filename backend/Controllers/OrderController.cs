using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Order;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =====================================================
        // 1. GET ALL ORDERS
        // GET: api/Order
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var orders =
                await unitOfWork.Orders.GetAllAsync();

            var result = orders.Select(o => new
            {
                orderId = o.OrderId,
                orderNumber = o.OrderNumber,

                supplierId = o.SupplierId,
                supplierName = o.Supplier?.SupplierName,

                warehouseId = o.WarehouseId,
                warehouseName = o.Warehouse?.Warehouse_Name,

                orderDate = o.OrderDate,
                expectedDate = o.ExpectedDate,

                status = o.Status,
                priority = o.Priority,

                createdBy = o.CreatedBy,
                createdByUser = o.CreatedByUser?.User_Name,

                approvedBy = o.ApprovedBy,
                approvedByUser = o.ApprovedByUser?.User_Name,

                notes = o.Notes,

                subtotal = o.Subtotal,
                taxAmount = o.TaxAmount,
                totalAmount = o.TotalAmount,

                createdAt = o.CreatedAt,
                updatedAt = o.UpdatedAt,

                itemsCount = o.OrderItems.Count
            });

            return Ok(result);
        }


        // =====================================================
        // 2. GET ORDER BY ID
        // GET: api/Order/1
        // =====================================================

        [HttpGet("GetOrderById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order =
                await unitOfWork.Orders.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(new
            {
                orderId = order.OrderId,
                orderNumber = order.OrderNumber,

                supplierId = order.SupplierId,
                supplierName = order.Supplier?.SupplierName,

                warehouseId = order.WarehouseId,
                warehouseName = order.Warehouse?.Warehouse_Name,

                orderDate = order.OrderDate,
                expectedDate = order.ExpectedDate,

                status = order.Status,
                priority = order.Priority,

                createdBy = order.CreatedBy,
                createdByUser = order.CreatedByUser?.User_Name,

                approvedBy = order.ApprovedBy,
                approvedByUser = order.ApprovedByUser?.User_Name,

                notes = order.Notes,

                subtotal = order.Subtotal,
                taxAmount = order.TaxAmount,
                totalAmount = order.TotalAmount,

                createdAt = order.CreatedAt,
                updatedAt = order.UpdatedAt,

                items = order.OrderItems.Select(i => new
                {
                    orderItemId = i.OrderItemId,

                    productId = i.ProductId,
                    productName = i.Product?.ProductName,
                    sku = i.Product?.SKU,

                    quantity = i.Quantity,
                    unitPrice = i.UnitPrice,
                    taxRate = i.TaxRate,
                    totalPrice = i.TotalPrice,
                    receivedQuantity = i.ReceivedQuantity,

                    notes = i.Notes
                })
            });
        }


        // =====================================================
        // 3. GET BY ORDER NUMBER
        // GET: api/Order/by-number/PO-001
        // =====================================================

        [HttpGet("GetOrderby-number/{orderNumber}")]
        public async Task<IActionResult> GetByOrderNumber(
            string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return BadRequest(
                    "Order number is required.");
            }

            var order =
                await unitOfWork.Orders
                    .GetByOrderNumberAsync(
                        orderNumber.Trim());

            if (order == null)
            {
                return NotFound(
                    "Order not found.");
            }

            return Ok(new
            {
                orderId = order.OrderId,
                orderNumber = order.OrderNumber,

                supplierId = order.SupplierId,
                supplierName = order.Supplier?.SupplierName,

                warehouseId = order.WarehouseId,
                warehouseName = order.Warehouse?.Warehouse_Name,

                orderDate = order.OrderDate,
                expectedDate = order.ExpectedDate,

                status = order.Status,
                priority = order.Priority,

                subtotal = order.Subtotal,
                taxAmount = order.TaxAmount,
                totalAmount = order.TotalAmount,

                items = order.OrderItems.Select(i => new
                {
                    orderItemId = i.OrderItemId,
                    productId = i.ProductId,
                    productName = i.Product?.ProductName,
                    sku = i.Product?.SKU,
                    quantity = i.Quantity,
                    unitPrice = i.UnitPrice,
                    taxRate = i.TaxRate,
                    totalPrice = i.TotalPrice,
                    receivedQuantity = i.ReceivedQuantity,
                    notes = i.Notes
                })
            });
        }


        // =====================================================
        // 4. GET BY SUPPLIER
        // GET: api/Order/supplier/1
        // =====================================================

        [HttpGet("GetOrderBysupplier/{supplierId:int}")]
        public async Task<IActionResult> GetBySupplierId(
            int supplierId)
        {
            var supplier =
                await unitOfWork.Suppliers
                    .GetByIdAsync(supplierId);

            if (supplier == null)
            {
                return NotFound(
                    "Supplier not found.");
            }

            var orders =
                await unitOfWork.Orders
                    .GetBySupplierIdAsync(
                        supplierId);

            var result = orders.Select(o => new
            {
                orderId = o.OrderId,
                orderNumber = o.OrderNumber,

                supplierId = o.SupplierId,
                supplierName = o.Supplier?.SupplierName,

                warehouseId = o.WarehouseId,
                warehouseName = o.Warehouse?.Warehouse_Name,

                orderDate = o.OrderDate,
                expectedDate = o.ExpectedDate,

                status = o.Status,
                priority = o.Priority,

                subtotal = o.Subtotal,
                taxAmount = o.TaxAmount,
                totalAmount = o.TotalAmount,

                itemsCount = o.OrderItems.Count,

                createdAt = o.CreatedAt,
                updatedAt = o.UpdatedAt
            });

            return Ok(result);
        }


        // =====================================================
        // 5. GET BY WAREHOUSE
        // GET: api/Order/warehouse/1
        // =====================================================

        [HttpGet("GetOrderBywarehouse/{warehouseId:int}")]
        public async Task<IActionResult> GetByWarehouseId(
            int warehouseId)
        {
            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(warehouseId);

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }

            var orders =
                await unitOfWork.Orders
                    .GetbywarehouseId(
                        warehouseId);

            var result = orders.Select(o => new
            {
                orderId = o.OrderId,
                orderNumber = o.OrderNumber,

                supplierId = o.SupplierId,
                supplierName = o.Supplier?.SupplierName,

                warehouseId = o.WarehouseId,
                warehouseName = o.Warehouse?.Warehouse_Name,

                orderDate = o.OrderDate,
                expectedDate = o.ExpectedDate,

                status = o.Status,
                priority = o.Priority,

                subtotal = o.Subtotal,
                taxAmount = o.TaxAmount,
                totalAmount = o.TotalAmount,

                itemsCount = o.OrderItems.Count,

                createdAt = o.CreatedAt,
                updatedAt = o.UpdatedAt
            });

            return Ok(result);
        }


        // =====================================================
        // 6. GET BY STATUS
        // GET: api/Order/status/Pending
        // =====================================================

        [HttpGet("GetorderBystatus/{status}")]
        public async Task<IActionResult> GetByStatus(
            OrderStatus status)
        {
            if (!Enum.IsDefined(
                    typeof(OrderStatus),
                    status))
            {
                return BadRequest(
                    "Invalid order status.");
            }

            var orders =
                await unitOfWork.Orders
                    .GetByStatusAsync(status);

            var result = orders.Select(o => new
            {
                orderId = o.OrderId,
                orderNumber = o.OrderNumber,

                supplierId = o.SupplierId,
                supplierName = o.Supplier?.SupplierName,

                warehouseId = o.WarehouseId,
                warehouseName = o.Warehouse?.Warehouse_Name,

                orderDate = o.OrderDate,
                expectedDate = o.ExpectedDate,

                status = o.Status,
                priority = o.Priority,

                subtotal = o.Subtotal,
                taxAmount = o.TaxAmount,
                totalAmount = o.TotalAmount,

                itemsCount = o.OrderItems.Count,

                createdAt = o.CreatedAt,
                updatedAt = o.UpdatedAt
            });

            return Ok(result);
        }


        // =====================================================
        // 7. CREATE ORDER
        // POST: api/Order
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderNumber = $"PO-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return BadRequest(
                    "Order number is required.");
            }


            // =================================================
            // CHECK DUPLICATE ORDER NUMBER
            // =================================================

            var exists =
                await unitOfWork.Orders
                    .OrderNumberExistsAsync(
                        orderNumber);

            if (exists)
            {
                return Conflict(
                    "Order number already exists.");
            }


            // =================================================
            // CHECK SUPPLIER
            // =================================================

            var supplier =
                await unitOfWork.Suppliers
                    .GetByIdAsync(
                        dto.SupplierId);

            if (supplier == null)
            {
                return BadRequest(
                    "Supplier not found.");
            }


            // =================================================
            // CHECK WAREHOUSE
            // =================================================

            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(
                        dto.WarehouseId);

            if (warehouse == null)
            {
                return BadRequest(
                    "Warehouse not found.");
            }


            // =================================================
            // CHECK USER
            // =================================================

            var user =
                await unitOfWork.User
                    .GetByIdAsync(
                        dto.CreatedBy);

            if (user == null)
            {
                return BadRequest(
                    "CreatedBy user not found.");
            }


            // =================================================
            // CREATE ORDER
            // =================================================

            var order = new Order
            {
                OrderNumber = orderNumber,

                SupplierId = dto.SupplierId,

                OrderDate =
                    DateTimeOffset.UtcNow,

                ExpectedDate =
                    dto.ExpectedDate,

                Status =
                    dto.Status,

                Priority =
                    dto.Priority,

                WarehouseId =
                    dto.WarehouseId,

                CreatedBy =
                    dto.CreatedBy,

                Notes =
                    string.IsNullOrWhiteSpace(dto.Notes)
                        ? null
                        : dto.Notes.Trim(),

                Subtotal = 0,

                TaxAmount = 0,

                TotalAmount = 0,

                CreatedAt =
                    DateTimeOffset.UtcNow
            };

            await unitOfWork.Orders
                .AddAsync(order);

            await unitOfWork.SaveAsync();


            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = order.OrderId
                },
                new
                {
                    message =
                        "Order created successfully.",

                    orderId =
                        order.OrderId,

                    orderNumber =
                        order.OrderNumber
                });
        }


        // =====================================================
        // 8. UPDATE ORDER
        // PUT: api/Order/1
        // =====================================================

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order =
                await unitOfWork.Orders
                    .GetByIdAsync(id);

            if (order == null)
            {
                return NotFound(
                    "Order not found.");
            }


            // =================================================
            // ORDER NUMBER
            // =================================================



            // =================================================
            // SUPPLIER
            // =================================================

            if (dto.SupplierId.HasValue)
            {
                var supplier =
                    await unitOfWork.Suppliers
                        .GetByIdAsync(
                            dto.SupplierId.Value);

                if (supplier == null)
                {
                    return BadRequest(
                        "Supplier not found.");
                }

                order.SupplierId =
                    dto.SupplierId.Value;
            }


            // =================================================
            // WAREHOUSE
            // =================================================

            if (dto.WarehouseId.HasValue)
            {
                var warehouse =
                    await unitOfWork.Warehouses
                        .GetByIdAsync(
                            dto.WarehouseId.Value);

                if (warehouse == null)
                {
                    return BadRequest(
                        "Warehouse not found.");
                }

                order.WarehouseId =
                    dto.WarehouseId.Value;
            }


            // =================================================
            // STATUS
            // =================================================

            if (dto.Status.HasValue)
            {
                order.Status =
                    dto.Status.Value;
            }


            // =================================================
            // PRIORITY
            // =================================================

            if (dto.Priority.HasValue)
            {
                order.Priority =
                    dto.Priority.Value;
            }


            // =================================================
            // EXPECTED DATE
            // =================================================

            if (dto.ExpectedDate.HasValue)
            {
                order.ExpectedDate =
                    dto.ExpectedDate.Value;
            }


            // =================================================
            // APPROVED BY
            // =================================================

            if (dto.ApprovedBy.HasValue)
            {
                var user =
                    await unitOfWork.User
                        .GetByIdAsync(
                            dto.ApprovedBy.Value);

                if (user == null)
                {
                    return BadRequest(
                        "ApprovedBy user not found.");
                }

                order.ApprovedBy =
                    dto.ApprovedBy.Value;
            }


            // =================================================
            // NOTES
            // =================================================

            if (dto.Notes != null)
            {
                order.Notes =
                    string.IsNullOrWhiteSpace(dto.Notes)
                        ? null
                        : dto.Notes.Trim();
            }

            order.UpdatedAt =
                DateTimeOffset.UtcNow;


            unitOfWork.Orders.Update(order);

            await unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Order updated successfully.",

                orderId =
                    order.OrderId,

                orderNumber =
                    order.OrderNumber,

                status =
                    order.Status,

                priority =
                    order.Priority,

                updatedAt =
                    order.UpdatedAt
            });
        }


        // =====================================================
        // 9. DELETE ORDER
        // DELETE: api/Order/1
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var order =
                await unitOfWork.Orders
                    .GetByIdAsync(id);

            if (order == null)
            {
                return NotFound(
                    "Order not found.");
            }

            unitOfWork.Orders.Delete(order);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Order deleted successfully.",

                orderId =
                    order.OrderId
            });
        }
    }
}