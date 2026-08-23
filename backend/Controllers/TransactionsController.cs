using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using whm.DTOs;
using whm.Models;
using whm.Repositories.Interfaces;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly DataBaseContext db;

        public TransactionsController(
            IUnitOfWork unitOfWork,
            DataBaseContext db)
        {
            this.unitOfWork = unitOfWork;
            this.db = db;
        }

        // =========================================================
        // 1. CREATE TRANSACTION
        // POST: api/Transactions
        // =========================================================

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTransaction(
            CreateTransactionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // =====================================================
            // Get logged-in user from JWT
            // =====================================================

            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) ||
                !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user token.");
            }

            // Check User
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.User_Id == userId);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // =====================================================
            // Check Product
            // =====================================================

            var product = await db.Products
                .FirstOrDefaultAsync(
                    p => p.ProductId == dto.Product_Id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // =====================================================
            // Check Unit
            // =====================================================

            var unit = await db.Units
                .FirstOrDefaultAsync(
                    u => u.Unit_Id == dto.Unit_Id);

            if (unit == null)
            {
                return BadRequest("Unit not found.");
            }

            // =====================================================
            // Validate Quantity
            // =====================================================

            if (dto.Quantity <= 0)
            {
                return BadRequest(
                    "Quantity must be greater than zero.");
            }

            // =====================================================
            // Validate Transaction Type
            // =====================================================

            if (dto.TransactionType < 1 ||
                dto.TransactionType > 3)
            {
                return BadRequest(
                    "Invalid transaction type. " +
                    "Use 1 = IN, 2 = OUT, 3 = TRANSFER.");
            }

            // =====================================================
            // IN
            // =====================================================

            if (dto.TransactionType == 1)
            {
                if (!dto.ToBinId.HasValue)
                {
                    return BadRequest(
                        "ToBinId is required for IN transaction.");
                }

                var toBin = await db.Bins
                    .FirstOrDefaultAsync(
                        b => b.Bin_Id == dto.ToBinId.Value);

                if (toBin == null)
                {
                    return BadRequest("To Bin not found.");
                }

                // Find existing stock
                var stock = await db.Stocks
                    .FirstOrDefaultAsync(s =>
                        s.ProductId == dto.Product_Id &&
                        s.Bin_Id == dto.ToBinId.Value);

                // Create stock if it doesn't exist
                if (stock == null)
                {
                    stock = new Stock
                    {
                        ProductId = dto.Product_Id,
                        Bin_Id = dto.ToBinId.Value,
                        Quantity = dto.Quantity,
                        IsActive = true,
                        CreateAt = DateTime.UtcNow,
                        LastUpdatedAt = DateTime.UtcNow
                    };

                    db.Stocks.Add(stock);
                }
                else
                {
                    stock.Quantity += dto.Quantity;
                    stock.LastUpdatedAt = DateTime.UtcNow;
                    stock.IsActive = true;
                }
            }

            // =====================================================
            // OUT
            // =====================================================

            else if (dto.TransactionType == 2)
            {
                if (!dto.FromBinId.HasValue)
                {
                    return BadRequest(
                        "FromBinId is required for OUT transaction.");
                }

                var fromBin = await db.Bins
                    .FirstOrDefaultAsync(
                        b => b.Bin_Id == dto.FromBinId.Value);

                if (fromBin == null)
                {
                    return BadRequest("From Bin not found.");
                }

                // Find stock
                var stock = await db.Stocks
                    .FirstOrDefaultAsync(s =>
                        s.ProductId == dto.Product_Id &&
                        s.Bin_Id == dto.FromBinId.Value);

                if (stock == null)
                {
                    return BadRequest(
                        "No stock found for this product in the selected bin.");
                }

                // Check quantity
                if (stock.Quantity < dto.Quantity)
                {
                    return BadRequest(new
                    {
                        message = "Insufficient stock.",
                        availableQuantity = stock.Quantity,
                        requestedQuantity = dto.Quantity
                    });
                }

                // Decrease stock
                stock.Quantity -= dto.Quantity;
                stock.LastUpdatedAt = DateTime.UtcNow;

                if (stock.Quantity == 0)
                {
                    stock.IsActive = false;
                }
            }

            // =====================================================
            // TRANSFER
            // =====================================================

            else if (dto.TransactionType == 3)
            {
                if (!dto.FromBinId.HasValue)
                {
                    return BadRequest(
                        "FromBinId is required for TRANSFER.");
                }

                if (!dto.ToBinId.HasValue)
                {
                    return BadRequest(
                        "ToBinId is required for TRANSFER.");
                }

                if (dto.FromBinId == dto.ToBinId)
                {
                    return BadRequest(
                        "FromBin and ToBin cannot be the same.");
                }

                // Check From Bin
                var fromBin = await db.Bins
                    .FirstOrDefaultAsync(
                        b => b.Bin_Id == dto.FromBinId.Value);

                if (fromBin == null)
                {
                    return BadRequest("From Bin not found.");
                }

                // Check To Bin
                var toBin = await db.Bins
                    .FirstOrDefaultAsync(
                        b => b.Bin_Id == dto.ToBinId.Value);

                if (toBin == null)
                {
                    return BadRequest("To Bin not found.");
                }

                // Get source stock
                var fromStock = await db.Stocks
                    .FirstOrDefaultAsync(s =>
                        s.ProductId == dto.Product_Id &&
                        s.Bin_Id == dto.FromBinId.Value);

                if (fromStock == null)
                {
                    return BadRequest(
                        "No stock found in the source bin.");
                }

                // Check quantity
                if (fromStock.Quantity < dto.Quantity)
                {
                    return BadRequest(new
                    {
                        message = "Insufficient stock.",
                        availableQuantity = fromStock.Quantity,
                        requestedQuantity = dto.Quantity
                    });
                }

                // Remove from source
                fromStock.Quantity -= dto.Quantity;
                fromStock.LastUpdatedAt = DateTime.UtcNow;

                if (fromStock.Quantity == 0)
                {
                    fromStock.IsActive = false;
                }

                // Get destination stock
                var toStock = await db.Stocks
                    .FirstOrDefaultAsync(s =>
                        s.ProductId == dto.Product_Id &&
                        s.Bin_Id == dto.ToBinId.Value);

                // Create destination stock
                if (toStock == null)
                {
                    toStock = new Stock
                    {
                        ProductId = dto.Product_Id,
                        Bin_Id = dto.ToBinId.Value,
                        Quantity = dto.Quantity,
                        IsActive = true,
                        CreateAt = DateTime.UtcNow,
                        LastUpdatedAt = DateTime.UtcNow
                    };

                    db.Stocks.Add(toStock);
                }
                else
                {
                    toStock.Quantity += dto.Quantity;
                    toStock.LastUpdatedAt = DateTime.UtcNow;
                    toStock.IsActive = true;
                }
            }

            // =====================================================
            // CREATE TRANSACTION RECORD
            // =====================================================

            var transaction = new Transaction
            {
                Product_Id = dto.Product_Id,

                Quantity = dto.Quantity,

                Unit_Id = dto.Unit_Id,

                TransactionType =
                    (TransactionType)dto.TransactionType,

                FromBinId = dto.FromBinId,

                ToBinId = dto.ToBinId,

                User_Id = userId,

                Notes = dto.Notes,

                CreateAt = DateTimeOffset.UtcNow

            };

            // Use Repository
            await unitOfWork.Transactions
                .AddAsync(transaction);

            // Save everything
            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Transaction created successfully.",

                transactionId =
                    transaction.transaction_Id,

                productId =
                    transaction.Product_Id,

                quantity =
                    transaction.Quantity,

                unitId =
                    transaction.Unit_Id,

                transactionType =
                    transaction.TransactionType,

                fromBinId =
                    transaction.FromBinId,

                toBinId =
                    transaction.ToBinId,

                userId =
                    transaction.User_Id,

                createdAt =
                    transaction.CreateAt
            });
        }


        // =========================================================
        // 2. GET ALL TRANSACTIONS
        // GET: api/Transactions
        // =========================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions =
                await unitOfWork.Transactions
                    .GetAllAsync();

            var result = transactions
                .Select(t => new
                {
                    transactionId = t.transaction_Id,

                    productId = t.Product_Id,
                    productName = t.Product?.ProductName,

                    quantity = t.Quantity,

                    unitId = t.Unit_Id,
                    unitName = t.Unit?.Unit_Name,

                    transactionType = t.TransactionType,

                    fromBinId = t.FromBinId,
                    fromBinName = t.FromBin?.Bin_Name,

                    toBinId = t.ToBinId,
                    toBinName = t.ToBin?.Bin_Name,

                    userId = t.User_Id,
                    userName = t.User?.User_Name,

                    notes = t.Notes,

                    createdAt = t.CreateAt
                });

            return Ok(result);
        }


        // =========================================================
        // 3. GET TRANSACTION BY ID
        // GET: api/Transactions/{id}
        // =========================================================

        [HttpGet("Getbyid/{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction =
                await unitOfWork.Transactions
                    .GetByIdAsync(id);

            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            return Ok(new
            {
                transactionId =
                    transaction.transaction_Id,

                productId =
                    transaction.Product_Id,

                productName =
                    transaction.Product?.ProductName,

                quantity =
                    transaction.Quantity,

                unitId =
                    transaction.Unit_Id,

                unitName =
                    transaction.Unit?.Unit_Name,

                transactionType =
                    transaction.TransactionType,

                fromBinId =
                    transaction.FromBinId,

                fromBinName =
                    transaction.FromBin?.Bin_Name,

                toBinId =
                    transaction.ToBinId,

                toBinName =
                    transaction.ToBin?.Bin_Name,

                userId =
                    transaction.User_Id,

                userName =
                    transaction.User?.User_Name,

                notes =
                    transaction.Notes,

                createdAt =
                    transaction.CreateAt
            });
        }


        // =========================================================
        // 4. GET TRANSACTIONS BY PRODUCT
        // GET: api/Transactions/Product/{productId}
        // =========================================================

        [HttpGet("Product/{productId}GetTransactionById")]
        public async Task<IActionResult> GetTransactionsByProduct(
            int productId)
        {
            var productExists = await db.Products
                .AnyAsync(p => p.ProductId == productId);

            if (!productExists)
            {
                return NotFound("Product not found.");
            }

            var transactions =
                await unitOfWork.Transactions
                    .GetByProductIdAsync(productId);

            return Ok(transactions.Select(t => new
            {
                transactionId = t.transaction_Id,

                productId = t.Product_Id,
                productName = t.Product?.ProductName,

                quantity = t.Quantity,

                unitId = t.Unit_Id,
                unitName = t.Unit?.Unit_Name,

                transactionType = t.TransactionType,

                fromBinId = t.FromBinId,
                toBinId = t.ToBinId,

                userId = t.User_Id,
                userName = t.User?.User_Name,

                notes = t.Notes,

                createdAt = t.CreateAt
            }));
        }


        // =========================================================
        // 5. GET TRANSACTIONS BY BIN
        // GET: api/Transactions/Bin/{binId}
        // =========================================================

        [HttpGet("{binId}GetbyBin")]
        public async Task<IActionResult> GetTransactionsByBin(
            int binId)
        {
            var binExists = await db.Bins
                .AnyAsync(b => b.Bin_Id == binId);

            if (!binExists)
            {
                return NotFound("Bin not found.");
            }

            var transactions =
                await unitOfWork.Transactions
                    .GetByBinIdAsync(binId);

            return Ok(transactions.Select(t => new
            {
                transactionId = t.transaction_Id,

                productId = t.Product_Id,
                productName = t.Product?.ProductName,

                quantity = t.Quantity,

                unitId = t.Unit_Id,
                unitName = t.Unit?.Unit_Name,

                transactionType = t.TransactionType,

                fromBinId = t.FromBinId,
                toBinId = t.ToBinId,

                userId = t.User_Id,
                userName = t.User?.User_Name,

                notes = t.Notes,

                createdAt = t.CreateAt
            }));
        }


        // =========================================================
        // 6. GET TRANSACTIONS BY USER
        // GET: api/Transactions/User/{userId}
        // =========================================================

        [HttpGet("GetTransactionByUser/{userId}")]
        public async Task<IActionResult> GetTransactionsByUser(
            Guid userId)
        {
            var userExists = await db.Users
                .AnyAsync(u => u.User_Id == userId);

            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var transactions =
                await unitOfWork.Transactions
                    .GetByUserIdAsync(userId);

            return Ok(transactions.Select(t => new
            {
                transactionId = t.transaction_Id,

                productId = t.Product_Id,
                productName = t.Product?.ProductName,

                quantity = t.Quantity,

                unitId = t.Unit_Id,
                unitName = t.Unit?.Unit_Name,

                transactionType = t.TransactionType,

                fromBinId = t.FromBinId,
                toBinId = t.ToBinId,

                userId = t.User_Id,
                userName = t.User?.User_Name,

                notes = t.Notes,

                createdAt = t.CreateAt
            }));
        }


        // =========================================================
        // 7. GET TRANSACTIONS BY TYPE
        // GET: api/Transactions/Type/{type}
        // =========================================================

        [HttpGet("GetByType/{type}")]
        public async Task<IActionResult> GetTransactionsByType(
            int type)
        {
            if (type < 1 || type > 3)
            {
                return BadRequest(
                    "Invalid transaction type. " +
                    "Use 1 = IN, 2 = OUT, 3 = TRANSFER.");
            }

            var transactions =
                await unitOfWork.Transactions
                    .GetByTypeAsync(
                        (TransactionType)type);

            return Ok(transactions.Select(t => new
            {
                transactionId = t.transaction_Id,

                productId = t.Product_Id,
                productName = t.Product?.ProductName,

                quantity = t.Quantity,

                unitId = t.Unit_Id,
                unitName = t.Unit?.Unit_Name,

                transactionType = t.TransactionType,

                fromBinId = t.FromBinId,
                toBinId = t.ToBinId,

                userId = t.User_Id,
                userName = t.User?.User_Name,

                notes = t.Notes,

                createdAt = t.CreateAt
            }));
        }


        // =========================================================
        // 8. FILTER TRANSACTIONS
        // GET: api/Transactions/Filter
        // =========================================================

        [HttpGet("Filter")]
        public async Task<IActionResult> FilterTransactions(
            int? productId,
            Guid? userId,
            int? transactionType,
            int? fromBinId,
            int? toBinId,
            DateTimeOffset? fromDate,
            DateTimeOffset? toDate)
        {
            var query = db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .AsQueryable();

            // Product
            if (productId.HasValue)
            {
                query = query.Where(
                    t => t.Product_Id == productId.Value);
            }

            // User
            if (userId.HasValue)
            {
                query = query.Where(
                    t => t.User_Id == userId.Value);
            }

            // Transaction Type
            if (transactionType.HasValue)
            {
                if (transactionType < 1 ||
                    transactionType > 3)
                {
                    return BadRequest(
                        "Invalid transaction type.");
                }

                query = query.Where(
                    t => t.TransactionType ==
                        (TransactionType)transactionType.Value);
            }

            // From Bin
            if (fromBinId.HasValue)
            {
                query = query.Where(
                    t => t.FromBinId == fromBinId.Value);
            }

            // To Bin
            if (toBinId.HasValue)
            {
                query = query.Where(
                    t => t.ToBinId == toBinId.Value);
            }

            // From Date
            if (fromDate.HasValue)
            {
                query = query.Where(
                    t => t.CreateAt >= fromDate.Value);
            }

            // To Date
            if (toDate.HasValue)
            {
                query = query.Where(
                    t => t.CreateAt <= toDate.Value);
            }

            var result = await query
                .OrderByDescending(t => t.CreateAt)
                .Select(t => new
                {
                    transactionId = t.transaction_Id,

                    productId = t.Product_Id,
                    productName = t.Product.ProductName,

                    quantity = t.Quantity,

                    unitId = t.Unit_Id,
                    unitName = t.Unit.Unit_Name,

                    transactionType = t.TransactionType,

                    fromBinId = t.FromBinId,

                    toBinId = t.ToBinId,

                    userId = t.User_Id,
                    userName = t.User.User_Name,

                    notes = t.Notes,

                    createdAt = t.CreateAt
                })
                .ToListAsync();

            return Ok(result);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SearchTransactions(
    int? siteId,
    int? departmentId)
        {
            if (!siteId.HasValue && !departmentId.HasValue)
            {
                return BadRequest(
                    "Please provide SiteId or DepartmentId.");
            }


            // =====================================================
            // CHECK SITE
            // =====================================================

            if (siteId.HasValue)
            {
                var site = await unitOfWork.Sites
                    .GetByIdAsync(siteId.Value);

                if (site == null)
                {
                    return NotFound("Site not found.");
                }
            }


            // =====================================================
            // CHECK DEPARTMENT
            // =====================================================

            if (departmentId.HasValue)
            {
                var department =
                    await unitOfWork.Departments
                        .GetByIdAsync(departmentId.Value);

                if (department == null)
                {
                    return NotFound("Department not found.");
                }
            }


            // =====================================================
            // SEARCH
            // =====================================================

            var transactions =
                await unitOfWork.Transactions
                    .SearchBySiteAndDepartmentAsync(
                        siteId,
                        departmentId);


            if (!transactions.Any())
            {
                return NotFound(
                    "No transactions found matching the specified filters.");
            }


            // =====================================================
            // RESPONSE
            // =====================================================

            var result = transactions.Select(t => new
            {
                transactionId = t.transaction_Id,

                transactionType = t.TransactionType,

                quantity = t.Quantity,

                notes = t.Notes,

                createAt = t.CreateAt,


                // =================================================
                // PRODUCT
                // =================================================

                productId = t.Product_Id,

                productName = t.Product?.ProductName,

                sku = t.Product?.SKU,

                barcode = t.Product?.Barcode,

                categoryId = t.Product?.CategoryId,

                categoryName =
                    t.Product?.Category?.Category_Name,


                // =================================================
                // UNIT
                // =================================================

                unitId = t.Unit_Id,

                unitName = t.Unit?.Unit_Name,


                // =================================================
                // USER
                // =================================================

                userId = t.User_Id,

                userName = t.User?.User_Name,


                // =================================================
                // FROM BIN
                // =================================================

                fromBinId = t.FromBinId,

                fromBinName = t.FromBin?.Bin_Name,


                // =================================================
                // TO BIN
                // =================================================

                toBinId = t.ToBinId,

                toBinName = t.ToBin?.Bin_Name
            });


            return Ok(new
            {
                count = result.Count(),

                filters = new
                {
                    siteId,
                    departmentId
                },

                transactions = result
            });
        }
    }
}