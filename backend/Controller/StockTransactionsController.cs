using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/stock-transactions")]
    [Authorize]
    public class StockTransactionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockTransactionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL TRANSACTIONS
        // =====================================================

        [HttpGet]
        public async Task<ActionResult<List<StockTransactionResponseDTO>>> GetAll(
            [FromQuery] int? productId,
            [FromQuery] int? stockId,
            [FromQuery] int? warehouseId,
            [FromQuery] int? locationId,
            [FromQuery] string? transactionType,
            [FromQuery] DateTimeOffset? fromDate,
            [FromQuery] DateTimeOffset? toDate,
            [FromQuery] Guid? userId)
        {
            var transactions =
                await _unitOfWork.StockTransactions.GetAllAsync(
                    productId,
                    stockId,
                    warehouseId,
                    locationId,
                    transactionType,
                    fromDate,
                    toDate,
                    userId);

            var result = transactions.Select(MapToDTO).ToList();

            return Ok(result);
        }


        // =====================================================
        // GET TRANSACTION BY ID
        // =====================================================

        [HttpGet("{id:long}")]
        public async Task<ActionResult<StockTransactionResponseDTO>> GetById(
            long id)
        {
            var transaction =
                await _unitOfWork.StockTransactions.GetByIdAsync(id);

            if (transaction == null)
            {
                return NotFound(new
                {
                    message = "Stock transaction not found."
                });
            }

            return Ok(MapToDTO(transaction));
        }


        // =====================================================
        // MAPPING
        // =====================================================

        private static StockTransactionResponseDTO MapToDTO(
            StockTransaction transaction)
        {
            return new StockTransactionResponseDTO
            {
                TransactionId = transaction.TransactionId,

                ProductId = transaction.ProductId,
                ProductName = transaction.Product?.Name,
                SKU = transaction.Product?.SKU,

                StockId = transaction.StockId,
                StockCode = transaction.Stock?.StockCode,

                WarehouseId = transaction.Stock?.WarehouseId,

                LocationId = transaction.Stock?.LocationId,

                TransactionType = transaction.TransactionType,

                Quantity = transaction.Quantity,

                SourceLocationId =
                    transaction.SourceLocationId,

                SourceLocationName =
                    transaction.SourceLocation?.Name,

                DestinationLocationId =
                    transaction.DestinationLocationId,

                DestinationLocationName =
                    transaction.DestinationLocation?.Name,

                PerformedBy = transaction.PerformedBy,

                PerformerName =
                    transaction.Performer?.Name,

                CreatedAt = transaction.CreatedAt,

                ReferenceType = transaction.ReferenceType,

                ReferenceId = transaction.ReferenceId,

                Notes = transaction.Notes
            };
        }
    }
}