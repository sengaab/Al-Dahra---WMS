using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Partition;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PartitionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PartitionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =====================================================
        // GET /api/partitions
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetPartitions(
            [FromQuery] int? warehouseId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var partitions =
                await _unitOfWork.Partitions.GetAllAsync(
                    warehouseId,
                    search,
                    status,
                    page,
                    pageSize);

            return Ok(partitions);
        }

        // =====================================================
        // GET /api/partitions/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPartition(int id)
        {
            var partition =
                await _unitOfWork.Partitions.GetByIdAsync(id);

            if (partition == null)
            {
                return NotFound(new
                {
                    message = "Partition not found."
                });
            }

            return Ok(partition);
        }

        // =====================================================
        // GET /api/partitions/warehouse/{warehouseId}
        // =====================================================

        [HttpGet("warehouse/{warehouseId:int}")]
        public async Task<IActionResult> GetByWarehouse(
            int warehouseId)
        {
            var warehouse =
                await _unitOfWork.Warehouses
                    .GetEntityByIdAsync(warehouseId);

            if (warehouse == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }

            var partitions =
                await _unitOfWork.Partitions
                    .GetByWarehouseIdAsync(warehouseId);

            return Ok(partitions);
        }

        // =====================================================
        // GET /api/partitions/summary
        // =====================================================

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(
            [FromQuery] int? warehouseId = null)
        {
            if (warehouseId.HasValue)
            {
                var warehouse =
                    await _unitOfWork.Warehouses
                        .GetEntityByIdAsync(
                            warehouseId.Value);

                if (warehouse == null)
                {
                    return NotFound(new
                    {
                        message = "Warehouse not found."
                    });
                }
            }

            var summary =
                await _unitOfWork.Partitions
                    .GetSummaryAsync(warehouseId);

            return Ok(summary);
        }

        // =====================================================
        // POST /api/partitions
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreatePartition(
            [FromBody] CreatePartitionDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // =================================================
            // Validate Warehouse
            // =================================================

            var warehouse =
                await _unitOfWork.Warehouses
                    .GetEntityByIdAsync(dto.WarehouseId);

            if (warehouse == null)
            {
                return BadRequest(new
                {
                    message = "Warehouse not found."
                });
            }

            // =================================================
            // Validate Code
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Partition code is required."
                });
            }

            // =================================================
            // Validate Name
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Partition name is required."
                });
            }

            // =================================================
            // Create
            // =================================================

            var now = DateTimeOffset.UtcNow;

            var partition = new Partition
            {
                WarehouseId = dto.WarehouseId,

                Code = dto.Code.Trim(),

                Name = dto.Name.Trim(),

                Description = string.IsNullOrWhiteSpace(
                    dto.Description)
                    ? null
                    : dto.Description.Trim(),

                IsActive = true,

                CreatedAt = now,

                UpdatedAt = now
            };

            await _unitOfWork.Partitions
                .AddAsync(partition);

            await _unitOfWork.SaveAsync();

            var result =
                await _unitOfWork.Partitions
                    .GetByIdAsync(partition.PartitionId);

            return CreatedAtAction(
                nameof(GetPartition),
                new
                {
                    id = partition.PartitionId
                },
                result);
        }

        // =====================================================
        // PUT /api/partitions/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePartition(
            int id,
            [FromBody] UpdatePartitionDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var partition =
                await _unitOfWork.Partitions
                    .GetEntityByIdAsync(id);

            if (partition == null)
            {
                return NotFound(new
                {
                    message = "Partition not found."
                });
            }

            // =================================================
            // Calculate Warehouse
            // =================================================

            var warehouseId =
                dto.WarehouseId ??
                partition.WarehouseId;

            // =================================================
            // Validate Warehouse
            // =================================================

            var warehouse =
                await _unitOfWork.Warehouses
                    .GetEntityByIdAsync(warehouseId);

            if (warehouse == null)
            {
                return BadRequest(new
                {
                    message = "Warehouse not found."
                });
            }

            // =================================================
            // Validate Code
            // =================================================

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message =
                            "Partition code cannot be empty."
                    });
                }

                partition.Code = dto.Code.Trim();
            }

            // =================================================
            // Validate Name
            // =================================================

            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new
                    {
                        message =
                            "Partition name cannot be empty."
                    });
                }

                partition.Name = dto.Name.Trim();
            }

            // =================================================
            // Description
            // =================================================

            if (dto.Description != null)
            {
                partition.Description =
                    string.IsNullOrWhiteSpace(dto.Description)
                        ? null
                        : dto.Description.Trim();
            }

            // =================================================
            // Active
            // =================================================

            if (dto.IsActive.HasValue)
            {
                partition.IsActive =
                    dto.IsActive.Value;
            }

            // =================================================
            // Warehouse
            // =================================================

            partition.WarehouseId = warehouseId;

            partition.UpdatedAt =
                DateTimeOffset.UtcNow;

            _unitOfWork.Partitions
                .Update(partition);

            await _unitOfWork.SaveAsync();

            var result =
                await _unitOfWork.Partitions
                    .GetByIdAsync(id);

            return Ok(result);
        }

        // =====================================================
        // DELETE /api/partitions/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePartition(int id)
        {
            var partition =
                await _unitOfWork.Partitions
                    .GetEntityByIdAsync(id);

            if (partition == null)
            {
                return NotFound(new
                {
                    message = "Partition not found."
                });
            }

            // =================================================
            // Check Bins
            // =================================================

            var hasBins =
                await _unitOfWork.Bins
                    .ExistsByPartitionIdAsync(id);

            if (hasBins)
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this partition because it contains bins."
                });
            }

            // =================================================
            // Delete
            // =================================================

            _unitOfWork.Partitions
                .Delete(partition);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message =
                    "Partition deleted successfully."
            });
        }
    }
}