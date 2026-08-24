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
    public class OrderItemController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderItemController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =====================================================
        // 1. GET ALL ORDER ITEMS
        // GET: api/OrderItem
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var items =
                await unitOfWork.OrderItems.GetAllAsync();

            var result = items.Select(item => new
            {
                orderItemId = item.OrderItemId,

                orderId = item.OrderId,

                productId = item.ProductId,

                productName =
                    item.Product?.ProductName,

                sku =
                    item.Product?.SKU,

                quantity =
                    item.Quantity,

                unitPrice =
                    item.UnitPrice,

                taxRate =
                    item.TaxRate,

                totalPrice =
                    item.TotalPrice,

                receivedQuantity =
                    item.ReceivedQuantity,

                remainingQuantity =
                    item.Quantity - item.ReceivedQuantity,

                notes =
                    item.Notes
            });

            return Ok(result);
        }


        // =====================================================
        // 2. GET ORDER ITEM BY ID
        // GET: api/OrderItem/1
        // =====================================================

        [HttpGet("GetById{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item =
                await unitOfWork.OrderItems.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound("Order item not found.");
            }

            return Ok(new
            {
                orderItemId =
                    item.OrderItemId,

                orderId =
                    item.OrderId,

                productId =
                    item.ProductId,

                productName =
                    item.Product?.ProductName,

                sku =
                    item.Product?.SKU,

                quantity =
                    item.Quantity,

                unitPrice =
                    item.UnitPrice,

                taxRate =
                    item.TaxRate,

                totalPrice =
                    item.TotalPrice,

                receivedQuantity =
                    item.ReceivedQuantity,

                remainingQuantity =
                    item.Quantity -
                    item.ReceivedQuantity,

                notes =
                    item.Notes
            });
        }


        // =====================================================
        // 3. GET ITEMS BY ORDER
        // GET: api/OrderItem/order/1
        // =====================================================

        [HttpGet("GetByorder/{orderId:int}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var order =
                await unitOfWork.Orders
                    .GetByIdAsync(orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var items =
                await unitOfWork.OrderItems
                    .GetByOrderIdAsync(orderId);

            var result = items.Select(item => new
            {
                orderItemId =
                    item.OrderItemId,

                orderId =
                    item.OrderId,

                productId =
                    item.ProductId,

                productName =
                    item.Product?.ProductName,

                sku =
                    item.Product?.SKU,

                quantity =
                    item.Quantity,

                unitPrice =
                    item.UnitPrice,

                taxRate =
                    item.TaxRate,

                totalPrice =
                    item.TotalPrice,

                receivedQuantity =
                    item.ReceivedQuantity,

                remainingQuantity =
                    item.Quantity -
                    item.ReceivedQuantity,

                notes =
                    item.Notes
            });

            return Ok(result);
        }


        // =====================================================
        // 4. CREATE ORDER ITEM
        // POST: api/OrderItem
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            CreateOrderItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // VALIDATE QUANTITY
            // =================================================

            if (dto.Quantity <= 0)
            {
                return BadRequest(
                    "Quantity must be greater than zero.");
            }


            // =================================================
            // VALIDATE UNIT PRICE
            // =================================================

            if (dto.UnitPrice < 0)
            {
                return BadRequest(
                    "Unit price cannot be negative.");
            }


            // =================================================
            // VALIDATE TAX
            // =================================================

            if (dto.TaxRate < 0 ||
                dto.TaxRate > 100)
            {
                return BadRequest(
                    "Tax rate must be between 0 and 100.");
            }


            // =================================================
            // VALIDATE RECEIVED QUANTITY
            // =================================================

            if (dto.ReceivedQuantity < 0)
            {
                return BadRequest(
                    "Received quantity cannot be negative.");
            }

            if (dto.ReceivedQuantity > dto.Quantity)
            {
                return BadRequest(
                    "Received quantity cannot be greater than quantity.");
            }


            // =================================================
            // CHECK ORDER
            // =================================================

            var order =
                await unitOfWork.Orders
                    .GetByIdAsync(dto.OrderId);

            if (order == null)
            {
                return BadRequest(
                    "Order not found.");
            }


            // =================================================
            // CHECK PRODUCT
            // =================================================

            var product =
                await unitOfWork.Products
                    .GetByIdAsync(dto.ProductId);

            if (product == null)
            {
                return BadRequest(
                    "Product not found.");
            }


            // =================================================
            // CALCULATE TOTAL
            // =================================================

            var subtotal =
                dto.Quantity *
                dto.UnitPrice;

            var taxAmount =
                subtotal *
                (dto.TaxRate / 100m);

            var totalPrice =
                subtotal +
                taxAmount;


            // =================================================
            // CREATE ORDER ITEM
            // =================================================

            var item = new OrderItem
            {
                OrderId =
                    dto.OrderId,

                ProductId =
                    dto.ProductId,

                Quantity =
                    dto.Quantity,

                UnitPrice =
                    dto.UnitPrice,

                TaxRate =
                    dto.TaxRate,

                TotalPrice =
                    totalPrice,

                ReceivedQuantity =
                    dto.ReceivedQuantity,

                Notes =
                    string.IsNullOrWhiteSpace(dto.Notes)
                        ? null
                        : dto.Notes.Trim()
            };


            // =================================================
            // SAVE ITEM
            // =================================================

            await unitOfWork.OrderItems
                .AddAsync(item);


            // =================================================
            // UPDATE ORDER TOTALS
            // =================================================

            var orderItems =
                await unitOfWork.OrderItems
                    .GetByOrderIdAsync(dto.OrderId);

            var subtotalAmount =
                orderItems.Sum(x =>
                    x.Quantity *
                    x.UnitPrice);

            var taxAmountTotal =
                orderItems.Sum(x =>
                    x.Quantity *
                    x.UnitPrice *
                    (x.TaxRate / 100m));

            order.Subtotal =
                subtotalAmount +
                (dto.Quantity * dto.UnitPrice);

            order.TaxAmount =
                taxAmountTotal +
                taxAmount;

            order.TotalAmount =
                order.Subtotal +
                order.TaxAmount;

            order.UpdatedAt =
                DateTimeOffset.UtcNow;

            unitOfWork.Orders.Update(order);

            await unitOfWork.SaveAsync();


            // =================================================
            // RESPONSE
            // =================================================

            return CreatedAtAction(
                nameof(GetById),

                new
                {
                    id = item.OrderItemId
                },

                new
                {
                    message =
                        "Order item created successfully.",

                    orderItemId =
                        item.OrderItemId,

                    orderId =
                        item.OrderId,

                    productId =
                        item.ProductId,

                    quantity =
                        item.Quantity,

                    unitPrice =
                        item.UnitPrice,

                    taxRate =
                        item.TaxRate,

                    totalPrice =
                        item.TotalPrice,

                    receivedQuantity =
                        item.ReceivedQuantity,

                    remainingQuantity =
                        item.Quantity -
                        item.ReceivedQuantity,

                    notes =
                        item.Notes
                });
        }


        // =====================================================
        // 5. UPDATE ORDER ITEM
        // PUT: api/OrderItem/1
        // =====================================================

        [HttpPut("Update{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateOrderItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // GET ITEM
            // =================================================

            var item =
                await unitOfWork.OrderItems
                    .GetByIdAsync(id);

            if (item == null)
            {
                return NotFound(
                    "Order item not found.");
            }


            // =================================================
            // CHECK VALUES
            // =================================================

            if (dto.Quantity.HasValue &&
                dto.Quantity.Value <= 0)
            {
                return BadRequest(
                    "Quantity must be greater than zero.");
            }

            if (dto.UnitPrice.HasValue &&
                dto.UnitPrice.Value < 0)
            {
                return BadRequest(
                    "Unit price cannot be negative.");
            }

            if (dto.TaxRate.HasValue &&
                (dto.TaxRate.Value < 0 ||
                 dto.TaxRate.Value > 100))
            {
                return BadRequest(
                    "Tax rate must be between 0 and 100.");
            }

            if (dto.ReceivedQuantity.HasValue &&
                dto.ReceivedQuantity.Value < 0)
            {
                return BadRequest(
                    "Received quantity cannot be negative.");
            }


            // =================================================
            // NEW VALUES
            // =================================================

            var newQuantity =
                dto.Quantity ??
                item.Quantity;

            var newUnitPrice =
                dto.UnitPrice ??
                item.UnitPrice;

            var newTaxRate =
                dto.TaxRate ??
                item.TaxRate;

            var newReceivedQuantity =
                dto.ReceivedQuantity ??
                item.ReceivedQuantity;


            if (newReceivedQuantity > newQuantity)
            {
                return BadRequest(
                    "Received quantity cannot be greater than quantity.");
            }


            // =================================================
            // CHECK PRODUCT
            // =================================================



            // =================================================
            // UPDATE PROVIDED VALUES
            // =================================================

            if (dto.Quantity.HasValue)
            {
                item.Quantity =
                    dto.Quantity.Value;
            }

            if (dto.UnitPrice.HasValue)
            {
                item.UnitPrice =
                    dto.UnitPrice.Value;
            }

            if (dto.TaxRate.HasValue)
            {
                item.TaxRate =
                    dto.TaxRate.Value;
            }

            if (dto.ReceivedQuantity.HasValue)
            {
                item.ReceivedQuantity =
                    dto.ReceivedQuantity.Value;
            }

            if (dto.Notes != null)
            {
                item.Notes =
                    string.IsNullOrWhiteSpace(dto.Notes)
                        ? null
                        : dto.Notes.Trim();
            }


            // =================================================
            // RECALCULATE TOTAL
            // =================================================

            item.TotalPrice =
                item.Quantity *
                item.UnitPrice *
                (1 + item.TaxRate / 100m);


            // =================================================
            // UPDATE
            // =================================================

            unitOfWork.OrderItems.Update(item);


            // =================================================
            // UPDATE ORDER TOTALS
            // =================================================

            var order =
                await unitOfWork.Orders
                    .GetByIdAsync(item.OrderId);

            if (order != null)
            {
                var orderItems =
                    await unitOfWork.OrderItems
                        .GetByOrderIdAsync(item.OrderId);

                order.Subtotal =
                    orderItems.Sum(x =>
                        x.Quantity *
                        x.UnitPrice);

                order.TaxAmount =
                    orderItems.Sum(x =>
                        x.Quantity *
                        x.UnitPrice *
                        (x.TaxRate / 100m));

                order.TotalAmount =
                    order.Subtotal +
                    order.TaxAmount;

                order.UpdatedAt =
                    DateTimeOffset.UtcNow;

                unitOfWork.Orders.Update(order);
            }


            // =================================================
            // SAVE
            // =================================================

            await unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Order item updated successfully.",

                orderItemId =
                    item.OrderItemId,

                orderId =
                    item.OrderId,

                productId =
                    item.ProductId,

                quantity =
                    item.Quantity,

                unitPrice =
                    item.UnitPrice,

                taxRate =
                    item.TaxRate,

                totalPrice =
                    item.TotalPrice,

                receivedQuantity =
                    item.ReceivedQuantity,

                remainingQuantity =
                    item.Quantity -
                    item.ReceivedQuantity,

                notes =
                    item.Notes
            });
        }


        // =====================================================
        // 6. DELETE ORDER ITEM
        // DELETE: api/OrderItem/1
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item =
                await unitOfWork.OrderItems
                    .GetByIdAsync(id);

            if (item == null)
            {
                return NotFound(
                    "Order item not found.");
            }


            var orderId =
                item.OrderId;


            // =================================================
            // DELETE ITEM
            // =================================================

            unitOfWork.OrderItems.Delete(item);


            // =================================================
            // UPDATE ORDER TOTALS
            // =================================================

            var order =
                await unitOfWork.Orders
                    .GetByIdAsync(orderId);

            if (order != null)
            {
                var orderItems =
                    await unitOfWork.OrderItems
                        .GetByOrderIdAsync(orderId);

                var remainingItems =
                    orderItems
                        .Where(x =>
                            x.OrderItemId != id)
                        .ToList();

                order.Subtotal =
                    remainingItems.Sum(x =>
                        x.Quantity *
                        x.UnitPrice);

                order.TaxAmount =
                    remainingItems.Sum(x =>
                        x.Quantity *
                        x.UnitPrice *
                        (x.TaxRate / 100m));

                order.TotalAmount =
                    order.Subtotal +
                    order.TaxAmount;

                order.UpdatedAt =
                    DateTimeOffset.UtcNow;

                unitOfWork.Orders.Update(order);
            }


            await unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Order item deleted successfully.",

                orderItemId =
                    id
            });
        }


        // =====================================================
        // 7. UPDATE RECEIVED QUANTITY
        // PATCH: api/OrderItem/1/received
        // =====================================================

        [HttpPatch("UPDATERECEIVEDQUANTITY{id:int}/received")]
        public async Task<IActionResult> UpdateReceivedQuantity(
            int id,
            decimal receivedQuantity)
        {
            var item =
                await unitOfWork.OrderItems
                    .GetByIdAsync(id);

            if (item == null)
            {
                return NotFound(
                    "Order item not found.");
            }

            if (receivedQuantity < 0)
            {
                return BadRequest(
                    "Received quantity cannot be negative.");
            }

            if (receivedQuantity > item.Quantity)
            {
                return BadRequest(
                    "Received quantity cannot be greater than ordered quantity.");
            }


            item.ReceivedQuantity =
                receivedQuantity;

            unitOfWork.OrderItems.Update(item);


            // =================================================
            // UPDATE ORDER STATUS
            // =================================================

            var order =
                await unitOfWork.Orders
                    .GetByIdAsync(item.OrderId);

            if (order != null)
            {
                var items =
                    await unitOfWork.OrderItems
                        .GetByOrderIdAsync(item.OrderId);

                var totalQuantity =
                    items.Sum(x => x.Quantity);

                var totalReceived =
                    items.Sum(x => x.ReceivedQuantity);

                if (totalReceived == 0)
                {
                    order.Status =
                        OrderStatus.Ordered;
                }
                else if (totalReceived < totalQuantity)
                {
                    order.Status =
                        OrderStatus.PartiallyReceived;
                }
                else
                {
                    order.Status =
                        OrderStatus.Received;
                }

                order.UpdatedAt =
                    DateTimeOffset.UtcNow;

                unitOfWork.Orders.Update(order);
            }


            await unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Received quantity updated successfully.",

                orderItemId =
                    item.OrderItemId,

                orderId =
                    item.OrderId,

                quantity =
                    item.Quantity,

                receivedQuantity =
                    item.ReceivedQuantity,

                remainingQuantity =
                    item.Quantity -
                    item.ReceivedQuantity
            });
        }
    }
}