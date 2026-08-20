using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public WarehousesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =========================================================
        // 1. CREATE WAREHOUSE
        // POST: api/Warehouses/Create
        // =========================================================

        [HttpPost("Create")]
        public async Task<IActionResult> CreateWarehouse(
            CreateWarehouseDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // =====================================================
            // NORMALIZE
            // =====================================================

            var warehouseName =
                dto.Warehouse_Name.Trim();

            var warehouseCode =
                dto.Warehouse_Code?.Trim();


            // =====================================================
            // CHECK SITE
            // =====================================================

            var site =
                await unitOfWork.Sites
                    .GetByIdAsync(dto.Site_Id);

            if (site == null)
            {
                return BadRequest(
                    "Site not found.");
            }

            if (!site.IsActive)
            {
                return BadRequest(
                    "Cannot add warehouse to an inactive site.");
            }


            // =====================================================
            // CHECK WAREHOUSE CODE
            // =====================================================

            if (!string.IsNullOrWhiteSpace(warehouseCode))
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


            // =====================================================
            // CREATE WAREHOUSE
            // =====================================================

            var warehouse = new Warehouse
            {
                Warehouse_Name =
                    warehouseName,

                Warehouse_Code =
                    warehouseCode,

                Warehouse_Description =
                    dto.Warehouse_Description?.Trim(),

                Site_Id =
                    dto.Site_Id,

                IsActive =
                    true
            };


            await unitOfWork.Warehouses
                .AddAsync(warehouse);

            await unitOfWork.SaveAsync();


            // =====================================================
            // RESPONSE
            // =====================================================

            return CreatedAtAction(
                nameof(GetWarehouseById),
                new
                {
                    id = warehouse.Warehouse_Id
                },
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

                    siteId =
                        warehouse.Site_Id,

                    isActive =
                        warehouse.IsActive
                });
        }


        // =========================================================
        // 2. GET ALL WAREHOUSES
        // GET: api/Warehouses/GetallWarehouses
        // =========================================================

        [HttpGet("GetallWarehouses")]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var warehouses =
                await unitOfWork.Warehouses
                    .GetAllAsync();

            var result =
                warehouses.Select(w => new
                {
                    warehouseId =
                        w.Warehouse_Id,

                    warehouseName =
                        w.Warehouse_Name,

                    warehouseCode =
                        w.Warehouse_Code,

                    description =
                        w.Warehouse_Description,

                    siteId =
                        w.Site_Id,

                    isActive =
                        w.IsActive
                });

            return Ok(result);
        }


        // =========================================================
        // 3. GET WAREHOUSE BY ID
        // GET: api/Warehouses/1/GetWarehousebyid
        // =========================================================

        [HttpGet("{id:int}/GetWarehousebyid")]
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

                siteId =
                    warehouse.Site_Id,

                isActive =
                    warehouse.IsActive
            });
        }


        // =========================================================
        // 4. GET WAREHOUSE BY CODE
        // GET: api/Warehouses/GetWarehouseCode/{code}
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

                siteId =
                    warehouse.Site_Id,

                isActive =
                    warehouse.IsActive
            });
        }


        // =========================================================
        // 5. UPDATE WAREHOUSE
        // PUT: api/Warehouses/UpdateWarehouseby/{id}
        // =========================================================

        [HttpPut("UpdateWarehouseby/{id:int}")]
        public async Task<IActionResult> UpdateWarehouse(
            int id,
            UpdateWarehouseDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =====================================================
            // GET WAREHOUSE
            // =====================================================

            var warehouse =
                await unitOfWork.Warehouses
                    .GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound(
                    "Warehouse not found.");
            }


            // =====================================================
            // CHECK SITE
            // =====================================================

            var site =
                await unitOfWork.Sites
                    .GetByIdAsync(dto.Site_Id);

            if (site == null)
            {
                return BadRequest(
                    "Site not found.");
            }

            if (!site.IsActive)
            {
                return BadRequest(
                    "Cannot assign warehouse to an inactive site.");
            }


            // =====================================================
            // NORMALIZE
            // =====================================================

            var warehouseName =
                dto.Warehouse_Name.Trim();

            var warehouseCode =
                dto.Warehouse_Code?.Trim();


            // =====================================================
            // CHECK DUPLICATE CODE
            // =====================================================

            if (!string.IsNullOrWhiteSpace(
                warehouseCode))
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


            // =====================================================
            // UPDATE
            // =====================================================

            warehouse.Warehouse_Name =
                warehouseName;

            warehouse.Warehouse_Code =
                warehouseCode;

            warehouse.Warehouse_Description =
                dto.Warehouse_Description?.Trim();

            warehouse.Site_Id =
                dto.Site_Id;


            unitOfWork.Warehouses
                .Update(warehouse);

            await unitOfWork.SaveAsync();


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

                siteId =
                    warehouse.Site_Id,

                isActive =
                    warehouse.IsActive
            });
        }


        // =========================================================
        // 6. DELETE WAREHOUSE
        // DELETE: api/Warehouses/DeleteWarehouseby/{id}
        // =========================================================

        [HttpDelete("DeleteWarehouseby/{id:int}")]
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


            // =====================================================
            // SOFT DELETE
            // =====================================================

            warehouse.IsActive =
                false;


            unitOfWork.Warehouses
                .Update(warehouse);

            await unitOfWork.SaveAsync();


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


        // =========================================================
        // 7. ACTIVATE WAREHOUSE
        // PATCH: api/Warehouses/ActivateWarehouseby/{id}
        // =========================================================

        [HttpPatch("ActivateWarehouseby/{id:int}")]
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


            warehouse.IsActive =
                true;


            unitOfWork.Warehouses
                .Update(warehouse);

            await unitOfWork.SaveAsync();


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

        [HttpPatch("{id:int}/deactivate")]
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


            warehouse.IsActive =
                false;


            unitOfWork.Warehouses
                .Update(warehouse);

            await unitOfWork.SaveAsync();


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