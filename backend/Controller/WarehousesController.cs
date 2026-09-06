using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Warehouse;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehousesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET: api/warehouses
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetWarehouses(
            [FromQuery] int? siteId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;

            var warehouses =
                await _unitOfWork.Warehouses.GetAllAsync(
                    siteId,
                    search,
                    status,
                    page,
                    pageSize);

            return Ok(warehouses);
        }


        // =====================================================
        // GET: api/warehouses/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetWarehouse(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }

            var warehouse =
                await _unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }

            return Ok(warehouse);
        }


        // =====================================================
        // POST: api/warehouses
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse(
            [FromBody] CreateWarehouseDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // =================================================
            // Validate Site
            // =================================================

            if (dto.SiteId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid SiteId."
                });
            }

            var site =
                await _unitOfWork.Sites
                    .GetEntityByIdAsync(dto.SiteId);

            if (site == null)
            {
                return BadRequest(new
                {
                    message = "Site not found."
                });
            }


            // =================================================
            // Validate Code
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Warehouse code is required."
                });
            }


            // =================================================
            // Validate Name
            // =================================================

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Warehouse name is required."
                });
            }


            // =================================================
            // Create Warehouse
            // =================================================

            var now = DateTimeOffset.UtcNow;

            var warehouse = new Warehouse
            {
                SiteId = dto.SiteId,

                Code = dto.Code.Trim(),

                Name = dto.Name.Trim(),

                Description =
                    string.IsNullOrWhiteSpace(dto.Description)
                        ? null
                        : dto.Description.Trim(),

                IsActive = true,

                CreatedAt = now,

                UpdatedAt = now
            };


            await _unitOfWork.Warehouses
                .AddAsync(warehouse);

            await _unitOfWork.SaveAsync();


            // =================================================
            // Get Created Warehouse
            // =================================================

            var result =
                await _unitOfWork.Warehouses
                    .GetByIdAsync(
                        warehouse.WarehouseId);


            return CreatedAtAction(
                nameof(GetWarehouse),
                new
                {
                    id = warehouse.WarehouseId
                },
                result);
        }


        // =====================================================
        // PUT: api/warehouses/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateWarehouse(
            int id,
            [FromBody] UpdateWarehouseDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            // =================================================
            // Get Warehouse
            // =================================================

            var warehouse =
                await _unitOfWork.Warehouses
                    .GetEntityByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }


            // =================================================
            // Update Site
            // =================================================

            if (dto.SiteId.HasValue)
            {
                if (dto.SiteId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid SiteId."
                    });
                }


                var site =
                    await _unitOfWork.Sites
                        .GetEntityByIdAsync(
                            dto.SiteId.Value);

                if (site == null)
                {
                    return BadRequest(new
                    {
                        message = "Site not found."
                    });
                }


                warehouse.SiteId =
                    dto.SiteId.Value;
            }


            // =================================================
            // Update Code
            // =================================================

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message =
                            "Warehouse code cannot be empty."
                    });
                }

                warehouse.Code =
                    dto.Code.Trim();
            }


            // =================================================
            // Update Name
            // =================================================

            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new
                    {
                        message =
                            "Warehouse name cannot be empty."
                    });
                }

                warehouse.Name =
                    dto.Name.Trim();
            }


            // =================================================
            // Update Description
            // =================================================

            if (dto.Description != null)
            {
                warehouse.Description =
                    string.IsNullOrWhiteSpace(
                        dto.Description)
                        ? null
                        : dto.Description.Trim();
            }


            // =================================================
            // Update Status
            // =================================================

            if (dto.IsActive.HasValue)
            {
                warehouse.IsActive =
                    dto.IsActive.Value;
            }


            // =================================================
            // Updated At
            // =================================================

            warehouse.UpdatedAt =
                DateTimeOffset.UtcNow;


            // =================================================
            // Save
            // =================================================

            _unitOfWork.Warehouses
                .Update(warehouse);

            await _unitOfWork.SaveAsync();


            // =================================================
            // Return Updated Warehouse
            // =================================================

            var result =
                await _unitOfWork.Warehouses
                    .GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE: api/warehouses/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWarehouse(
            int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            // =================================================
            // Get Warehouse
            // =================================================

            var warehouse =
                await _unitOfWork.Warehouses
                    .GetEntityByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }


            // =================================================
            // Get Warehouse Details
            // =================================================

            var warehouseDetails =
                await _unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouseDetails == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }


            // =================================================
            // Prevent Delete if Warehouse has Stock
            // =================================================

            var inventory =
                await _unitOfWork.Warehouses
                    .GetInventoryAsync(id);

            if (inventory.Count > 0)
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this warehouse because it contains stock."
                });
            }


            // =================================================
            // Prevent Delete if Warehouse has Bins
            // =================================================

            if (warehouseDetails.BinsCount > 0)
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this warehouse because it contains bins."
                });
            }


            // =================================================
            // Prevent Delete if Warehouse has Partitions
            // =================================================

            if (warehouseDetails.PartitionsCount > 0)
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this warehouse because it contains partitions."
                });
            }


            // =================================================
            // Delete Warehouse
            // =================================================

            _unitOfWork.Warehouses
                .Delete(warehouse);

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Warehouse deleted successfully."
            });
        }


        // =====================================================
        // GET: api/warehouses/{id}/inventory
        // =====================================================

        [HttpGet("{id:int}/inventory")]
        public async Task<IActionResult> GetInventory(
            int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            var warehouse =
                await _unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }


            var inventory =
                await _unitOfWork.Warehouses
                    .GetInventoryAsync(id);

            return Ok(inventory);
        }


        // =====================================================
        // GET: api/warehouses/{id}/stats
        // =====================================================

        [HttpGet("{id:int}/stats")]
        public async Task<IActionResult> GetStats(
            int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            var stats =
                await _unitOfWork.Warehouses
                    .GetStatsAsync(id);

            if (stats == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }


            return Ok(stats);
        }


        // =====================================================
        // GET: api/warehouses/{id}/occupancy
        // =====================================================

        [HttpGet("{id:int}/occupancy")]
        public async Task<IActionResult> GetOccupancy(
            int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            var occupancy =
                await _unitOfWork.Warehouses
                    .GetOccupancyAsync(id);

            if (occupancy == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }


            return Ok(occupancy);
        }
    }
}