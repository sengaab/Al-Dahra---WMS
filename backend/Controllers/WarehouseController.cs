using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.Repositories.Interfaces;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public WarehousesController(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // =========================================================
        // 1. CREATE WAREHOUSE
        // POST: api/Warehouses
        // =========================================================

        [HttpPost("Create")]
        public async Task<IActionResult> CreateWarehouse(
            CreateWarehouseDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Normalize
            var warehouseName =
                dto.Warehouse_Name.Trim();

            var warehouseCode =
                dto.Warehouse_Code?.Trim();

            // Check code
            if (!string.IsNullOrEmpty(warehouseCode))
            {
                var codeExists =
                    await unitOfWork.Warehouses
                        .CodeExistsAsync(warehouseCode);

                if (codeExists)
                {
                    return Conflict(
                        "Warehouse code already exists.");
                }
            }

            var warehouse = new Warehouse
            {
                Warehouse_Name = warehouseName,

                Warehouse_Code = warehouseCode,

                Warehouse_Description =
                    dto.Warehouse_Description?.Trim(),

                IsActive = true
            };

            await unitOfWork.Warehouses
                .AddAsync(warehouse);

            await unitOfWork.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWarehouseById),
                new { id = warehouse.Warehouse_Id },
                new
                {
                    message =
                        "Warehouse created successfully.",

                    warehouseId =
                        warehouse.Warehouse_Id,

                    warehouseName =
                        warehouse.Warehouse_Name,

                    warehouseCode =
                        warehouse.Warehouse_Code,

                    description =
                        warehouse.Warehouse_Description,

                    isActive =
                        warehouse.IsActive
                });
        }


        // =========================================================
        // 2. GET ALL WAREHOUSES
        // GET: api/Warehouses
        // =========================================================

        [HttpGet("GetallWarehouses")]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var warehouses =
                await unitOfWork.Warehouses
                    .GetAllAsync();

            var result = warehouses
                .Select(w => new
                {
                    warehouseId =
                        w.Warehouse_Id,

                    warehouseName =
                        w.Warehouse_Name,

                    warehouseCode =
                        w.Warehouse_Code,

                    description =
                        w.Warehouse_Description,

                    isActive =
                        w.IsActive
                });

            return Ok(result);
        }


        // =========================================================
        // 3. GET WAREHOUSE BY ID
        // GET: api/Warehouses/{id}
        // =========================================================

        [HttpGet("{id}/GetWarehousebyid")]
        public async Task<IActionResult> GetWarehouseById(
            int id)
        {
            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }

            return Ok(new
            {
                warehouseId =
                    warehouse.Warehouse_Id,

                warehouseName =
                    warehouse.Warehouse_Name,

                warehouseCode =
                    warehouse.Warehouse_Code,

                description =
                    warehouse.Warehouse_Description,

                isActive =
                    warehouse.IsActive
            });
        }


        // =========================================================
        // 4. GET WAREHOUSE BY CODE
        // GET: api/Warehouses/Code/{code}
        // =========================================================

        [HttpGet("GetWarehouseCode/{code}")]
        public async Task<IActionResult> GetWarehouseByCode(
            string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest(
                    "Warehouse code is required.");
            }

            var warehouse =
                await unitOfWork.Warehouses
                    .GetByCodeAsync(code.Trim());

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }

            return Ok(new
            {
                warehouseId =
                    warehouse.Warehouse_Id,

                warehouseName =
                    warehouse.Warehouse_Name,

                warehouseCode =
                    warehouse.Warehouse_Code,

                description =
                    warehouse.Warehouse_Description,

                isActive =
                    warehouse.IsActive
            });
        }


        // =========================================================
        // 5. UPDATE WAREHOUSE
        // PUT: api/Warehouses/{id}
        // =========================================================

        [HttpPut("UpdateWarehouseby{id}")]
        public async Task<IActionResult> UpdateWarehouse(
            int id,
            UpdateWarehouseDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }

            var warehouseName =
                dto.Warehouse_Name.Trim();

            var warehouseCode =
                dto.Warehouse_Code?.Trim();

            // Check duplicate code
            if (!string.IsNullOrEmpty(warehouseCode))
            {
                var codeExists =
                    await unitOfWork.Warehouses
                        .CodeExistsAsync(
                            warehouseCode,
                            id);

                if (codeExists)
                {
                    return Conflict(
                        "Warehouse code already exists.");
                }
            }

            warehouse.Warehouse_Name =
                warehouseName;

            warehouse.Warehouse_Code =
                warehouseCode;

            warehouse.Warehouse_Description =
                dto.Warehouse_Description?.Trim();

            unitOfWork.Warehouses
                .Update(warehouse);

            await unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                message =
                    "Warehouse updated successfully.",

                warehouseId =
                    warehouse.Warehouse_Id,

                warehouseName =
                    warehouse.Warehouse_Name,

                warehouseCode =
                    warehouse.Warehouse_Code,

                description =
                    warehouse.Warehouse_Description,

                isActive =
                    warehouse.IsActive
            });
        }


        // =========================================================
        // 6. DELETE WAREHOUSE
        // DELETE: api/Warehouses/{id}
        // =========================================================

        [HttpDelete("DeleteWarehouseby{id}")]
        public async Task<IActionResult> DeleteWarehouse(
            int id)
        {
            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }

            // Soft Delete
            warehouse.IsActive = false;

            unitOfWork.Warehouses
                .Update(warehouse);

            await unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                message =
                    "Warehouse deactivated successfully.",

                warehouseId =
                    warehouse.Warehouse_Id
            });
        }


        // =========================================================
        // 7. ACTIVATE WAREHOUSE
        // PATCH: api/Warehouses/{id}/activate
        // =========================================================

        [HttpPatch("ActivateWarehouseby{id}")]
        public async Task<IActionResult> ActivateWarehouse(
            int id)
        {
            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }

            if (warehouse.IsActive)
            {
                return BadRequest(
                    "Warehouse is already active.");
            }

            warehouse.IsActive = true;

            unitOfWork.Warehouses
                .Update(warehouse);

            await unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                message =
                    "Warehouse activated successfully.",

                warehouseId =
                    warehouse.Warehouse_Id,

                isActive =
                    warehouse.IsActive
            });
        }


        // =========================================================
        // 8. DEACTIVATE WAREHOUSE
        // PATCH: api/Warehouses/{id}/deactivate
        // =========================================================

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateWarehouse(
            int id)
        {
            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }

            if (!warehouse.IsActive)
            {
                return BadRequest(
                    "Warehouse is already inactive.");
            }

            warehouse.IsActive = false;

            unitOfWork.Warehouses
                .Update(warehouse);

            await unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                message =
                    "Warehouse deactivated successfully.",

                warehouseId =
                    warehouse.Warehouse_Id,

                isActive =
                    warehouse.IsActive
            });
        }
    }
}
