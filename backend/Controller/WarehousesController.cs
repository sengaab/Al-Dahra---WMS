using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Warehouse;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehousesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET /api/warehouses
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetWarehouses(
            [FromQuery] int? siteId,
            [FromQuery] string? search,
            [FromQuery] string? status,
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
        // GET /api/warehouses/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetWarehouse(int id)
        {
            var warehouse =
                await _unitOfWork.Warehouses.GetByIdAsync(id);

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
        // POST /api/warehouses
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse(
            [FromBody] CreateWarehouseDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (dto.SiteId <= 0)
            {
                return BadRequest(new
                {
                    message = "SiteId is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Warehouse code is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Warehouse name is required."
                });
            }

            var warehouse = new Warehouse
            {
                SiteId = dto.SiteId,
                Code = dto.Code.Trim(),
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim(),
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _unitOfWork.Warehouses.AddAsync(warehouse);

            await _unitOfWork.SaveAsync();

            var result =
                await _unitOfWork.Warehouses
                    .GetByIdAsync(warehouse.WarehouseId);

            return CreatedAtAction(
                nameof(GetWarehouse),
                new { id = warehouse.WarehouseId },
                result);
        }


        // =====================================================
        // PUT /api/warehouses/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateWarehouse(
            int id,
            [FromBody] UpdateWarehouseDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

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

            if (dto.SiteId.HasValue)
            {
                if (dto.SiteId.Value <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid SiteId."
                    });
                }

                warehouse.SiteId = dto.SiteId.Value;
            }

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message = "Warehouse code cannot be empty."
                    });
                }

                warehouse.Code = dto.Code.Trim();
            }

            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new
                    {
                        message = "Warehouse name cannot be empty."
                    });
                }

                warehouse.Name = dto.Name.Trim();
            }

            if (dto.Description != null)
            {
                warehouse.Description =
                    dto.Description.Trim();
            }

            if (dto.IsActive.HasValue)
            {
                warehouse.IsActive = dto.IsActive.Value;
            }

            warehouse.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.Warehouses.Update(warehouse);

            await _unitOfWork.SaveAsync();

            var result =
                await _unitOfWork.Warehouses
                    .GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE /api/warehouses/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
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

            _unitOfWork.Warehouses.Delete(warehouse);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Warehouse deleted successfully."
            });
        }


        // =====================================================
        // GET /api/warehouses/{id}/locations
        // =====================================================

        [HttpGet("{id:int}/locations")]
        public async Task<IActionResult> GetLocations(int id)
        {
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

            var locations =
                await _unitOfWork.Warehouses
                    .GetLocationsAsync(id);

            return Ok(locations);
        }


        // =====================================================
        // GET /api/warehouses/{id}/inventory
        // =====================================================

        [HttpGet("{id:int}/inventory")]
        public async Task<IActionResult> GetInventory(int id)
        {
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
        // GET /api/warehouses/{id}/stats
        // =====================================================

        [HttpGet("{id:int}/stats")]
        public async Task<IActionResult> GetStats(int id)
        {
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
        // GET /api/warehouses/{id}/occupancy
        // =====================================================

        [HttpGet("{id:int}/occupancy")]
        public async Task<IActionResult> GetOccupancy(int id)
        {
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