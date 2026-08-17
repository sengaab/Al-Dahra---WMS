using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using whm.DTOs;
using whm.Models;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly DataBaseContext db;

        public TransactionsController(DataBaseContext db)
        {
            this.db = db;
        }

        // =========================================================
        // 1. CREATE TRANSACTION
        // POST: api/Transactions
        // =========================================================

        [HttpPost]
        public IActionResult CreateTransaction(CreateTransactionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get logged-in user from JWT
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) ||
                !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user token.");
            }

            // Check User
            var user = db.Users
                .FirstOrDefault(u => u.User_Id == userId);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // Check Product
            var product = db.Products
                .FirstOrDefault(p => p.ProductId == dto.Product_Id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Check Unit
            var unit = db.Units
                .FirstOrDefault(u => u.Unit_Id == dto.Unit_Id);

            if (unit == null)
            {
                return BadRequest("Unit not found.");
            }

            // Check Transaction Type
            if (dto.TransactionType < 1 || dto.TransactionType > 3)
            {
                return BadRequest(
                    "Invalid transaction type. Use 1 = IN, 2 = OUT, 3 = TRANSFER."
                );
            }

            // =====================================================
            // IN
            // =====================================================

            if (dto.TransactionType == 1)
            {
                if (!dto.ToBinId.HasValue)
                {
                    return BadRequest(
                        "ToBinId is required for IN transaction."
                    );
                }

                var toBin = db.Bins
                    .FirstOrDefault(b => b.Bin_Id == dto.ToBinId.Value);

                if (toBin == null)
                {
                    return BadRequest("To Bin not found.");
                }

                // Find existing stock
                var stock = db.Stocks
                    .FirstOrDefault(s =>
                        s.ProductId == dto.Product_Id &&
                        s.Bin_Id == dto.ToBinId.Value);

                // If stock doesn't exist -> create it
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
                        "FromBinId is required for OUT transaction."
                    );
                }

                var fromBin = db.Bins
                    .FirstOrDefault(b =>
                        b.Bin_Id == dto.FromBinId.Value);

                if (fromBin == null)
                {
                    return BadRequest("From Bin not found.");
                }

                // Find stock
                var stock = db.Stocks
                    .FirstOrDefault(s =>
                        s.ProductId == dto.Product_Id &&
                        s.Bin_Id == dto.FromBinId.Value);

                if (stock == null)
                {
                    return BadRequest(
                        "No stock found for this product in the selected bin."
                    );
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

                // If stock becomes zero
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
                        "FromBinId is required for TRANSFER."
                    );
                }

                if (!dto.ToBinId.HasValue)
                {
                    return BadRequest(
                        "ToBinId is required for TRANSFER."
                    );
                }

                if (dto.FromBinId == dto.ToBinId)
                {
                    return BadRequest(
                        "FromBin and ToBin cannot be the same."
                    );
                }

                // Check From Bin
                var fromBin = db.Bins
                    .FirstOrDefault(b =>
                        b.Bin_Id == dto.FromBinId.Value);

                if (fromBin == null)
                {
                    return BadRequest("From Bin not found.");
                }

                // Check To Bin
                var toBin = db.Bins
                    .FirstOrDefault(b =>
                        b.Bin_Id == dto.ToBinId.Value);

                if (toBin == null)
                {
                    return BadRequest("To Bin not found.");
                }

                // Get source stock
                var fromStock = db.Stocks
                    .FirstOrDefault(s =>
                        s.ProductId == dto.Product_Id &&
                        s.Bin_Id == dto.FromBinId.Value);

                if (fromStock == null)
                {
                    return BadRequest(
                        "No stock found in the source bin."
                    );
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
                var toStock = db.Stocks
                    .FirstOrDefault(s =>
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

                TransactionType = (TransactionType)dto.TransactionType,

                FromBinId = dto.FromBinId,

                ToBinId = dto.ToBinId,

                User_Id = userId,

                Notes = dto.Notes,

                CreateAt = DateTimeOffset.UtcNow
            };

            db.Transactions.Add(transaction);

            // Save everything
            db.SaveChanges();

            return Ok(new
            {
                message = "Transaction created successfully.",

                transactionId = transaction.transaction_Id,

                productId = transaction.Product_Id,

                quantity = transaction.Quantity,

                unitId = transaction.Unit_Id,

                transactionType = transaction.TransactionType,

                fromBinId = transaction.FromBinId,

                toBinId = transaction.ToBinId,

                userId = transaction.User_Id,

                createdAt = transaction.CreateAt
            });
        }


        // =========================================================
        // 2. GET ALL TRANSACTIONS
        // GET: api/Transactions
        // =========================================================

        [HttpGet]
        public IActionResult GetAllTransactions()
        {
            var transactions = db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
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
                    fromBinName = t.FromBin != null
                        ? t.FromBin.Bin_Name
                        : null,

                    toBinId = t.ToBinId,
                    toBinName = t.ToBin != null
                        ? t.ToBin.Bin_Name
                        : null,

                    userId = t.User_Id,
                    userName = t.User.User_Name,

                    notes = t.Notes,

                    createdAt = t.CreateAt
                })
                .ToList();

            return Ok(transactions);
        }


        // =========================================================
        // 3. GET TRANSACTION BY ID
        // GET: api/Transactions/{id}
        // =========================================================

        [HttpGet("{id}")]
        public IActionResult GetTransactionById(int id)
        {
            var transaction = db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Include(t => t.FromBin)
                .Include(t => t.ToBin)
                .Where(t => t.transaction_Id == id)
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
                    fromBinName = t.FromBin != null
                        ? t.FromBin.Bin_Name
                        : null,

                    toBinId = t.ToBinId,
                    toBinName = t.ToBin != null
                        ? t.ToBin.Bin_Name
                        : null,

                    userId = t.User_Id,
                    userName = t.User.User_Name,

                    notes = t.Notes,

                    createdAt = t.CreateAt
                })
                .FirstOrDefault();

            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            return Ok(transaction);
        }


        // =========================================================
        // 4. GET TRANSACTIONS BY PRODUCT
        // GET: api/Transactions/Product/{productId}
        // =========================================================

        [HttpGet("Product/{productId}")]
        public IActionResult GetTransactionsByProduct(int productId)
        {
            var productExists = db.Products
                .Any(p => p.ProductId == productId);

            if (!productExists)
            {
                return NotFound("Product not found.");
            }

            var transactions = db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Where(t => t.Product_Id == productId)
                .OrderByDescending(t => t.CreateAt)
                .Select(t => new
                {
                    transactionId = t.transaction_Id,

                    productId = t.Product_Id,
                    productName = t.Product.ProductName,

                    quantity = t.Quantity,

                    unitName = t.Unit.Unit_Name,

                    transactionType = t.TransactionType,

                    fromBinId = t.FromBinId,

                    toBinId = t.ToBinId,

                    userId = t.User_Id,
                    userName = t.User.User_Name,

                    notes = t.Notes,

                    createdAt = t.CreateAt
                })
                .ToList();

            return Ok(transactions);
        }


        // =========================================================
        // 5. GET TRANSACTIONS BY BIN
        // GET: api/Transactions/Bin/{binId}
        // =========================================================

        [HttpGet("Bin/{binId}")]
        public IActionResult GetTransactionsByBin(int binId)
        {
            var binExists = db.Bins
                .Any(b => b.Bin_Id == binId);

            if (!binExists)
            {
                return NotFound("Bin not found.");
            }

            var transactions = db.Transactions
                .Include(t => t.Product)
                .Include(t => t.User)
                .Include(t => t.Unit)
                .Where(t =>
                    t.FromBinId == binId ||
                    t.ToBinId == binId)
                .OrderByDescending(t => t.CreateAt)
                .Select(t => new
                {
                    transactionId = t.transaction_Id,

                    productId = t.Product_Id,
                    productName = t.Product.ProductName,

                    quantity = t.Quantity,

                    unitName = t.Unit.Unit_Name,

                    transactionType = t.TransactionType,

                    fromBinId = t.FromBinId,

                    toBinId = t.ToBinId,

                    userId = t.User_Id,
                    userName = t.User.User_Name,

                    notes = t.Notes,

                    createdAt = t.CreateAt
                })
                .ToList();

            return Ok(transactions);
        }


        // =========================================================
        // 6. GET TRANSACTIONS BY USER
        // GET: api/Transactions/User/{userId}
        // =========================================================

        [HttpGet("User/{userId}")]
        public IActionResult GetTransactionsByUser(Guid userId)
        {
            var userExists = db.Users
                .Any(u => u.User_Id == userId);

            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var transactions = db.Transactions
                .Include(t => t.Product)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Where(t => t.User_Id == userId)
                .OrderByDescending(t => t.CreateAt)
                .Select(t => new
                {
                    transactionId = t.transaction_Id,

                    productId = t.Product_Id,
                    productName = t.Product.ProductName,

                    quantity = t.Quantity,

                    unitName = t.Unit.Unit_Name,

                    transactionType = t.TransactionType,

                    fromBinId = t.FromBinId,

                    toBinId = t.ToBinId,

                    userId = t.User_Id,
                    userName = t.User.User_Name,

                    notes = t.Notes,

                    createdAt = t.CreateAt
                })
                .ToList();

            return Ok(transactions);
        }


        // =========================================================
        // 7. GET TRANSACTIONS BY TYPE
        // GET: api/Transactions/Type/{type}
        // =========================================================

        [HttpGet("Type/{type}")]
        public IActionResult GetTransactionsByType(int type)
        {
            if (type < 1 || type > 3)
            {
                return BadRequest(
                    "Invalid transaction type. Use 1 = IN, 2 = OUT, 3 = TRANSFER."
                );
            }

            var transactions = db.Transactions
     .Include(t => t.Product)
     .Include(t => t.Unit)
     .Include(t => t.User)
     .Where(t => t.TransactionType == (TransactionType)type)
     .OrderByDescending(t => t.CreateAt)
                .Select(t => new
                {
                    transactionId = t.transaction_Id,

                    productId = t.Product_Id,
                    productName = t.Product.ProductName,

                    quantity = t.Quantity,

                    unitName = t.Unit.Unit_Name,

                    transactionType = t.TransactionType,

                    fromBinId = t.FromBinId,

                    toBinId = t.ToBinId,

                    userId = t.User_Id,
                    userName = t.User.User_Name,

                    notes = t.Notes,

                    createdAt = t.CreateAt
                })
                .ToList();

            return Ok(transactions);
        }


        // =========================================================
        // 8. FILTER TRANSACTIONS
        // GET: api/Transactions/Filter
        // =========================================================

        [HttpGet("Filter")]
        public IActionResult FilterTransactions(
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
                if (transactionType < 0 ||
                    transactionType > 3)
                {
                    return BadRequest(
                        "Invalid transaction type."
                    );
                }

                query = query.Where(
                    t => t.TransactionType == (TransactionType)transactionType.Value);
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

            var result = query
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
                .ToList();

            return Ok(result);
        }
    }
}