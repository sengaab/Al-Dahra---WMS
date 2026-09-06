using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Bin;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BinsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BinsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =========================================================
        // GET: api/bins
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> GetBins(
            [FromQuery] int? warehouseId = null,
            [FromQuery] int? partitionId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            // -----------------------------------------------------
            // Validate pagination
            // -----------------------------------------------------

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;


            // -----------------------------------------------------
            // Validate WarehouseId if provided
            // -----------------------------------------------------

            if (warehouseId.HasValue && warehouseId.Value <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            // -----------------------------------------------------
            // Validate PartitionId if provided
            // -----------------------------------------------------

            if (partitionId.HasValue && partitionId.Value <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid PartitionId."
                });
            }


            // -----------------------------------------------------
            // Get Bins
            // -----------------------------------------------------

            var bins = await _unitOfWork.Bins.GetAllAsync(
                warehouseId,
                partitionId,
                search,
                status,
                page,
                pageSize);


            return Ok(bins);
        }


        // =========================================================
        // GET: api/bins/{id}
        // =========================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBin(int id)
        {
            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }


            // -----------------------------------------------------
            // Get Bin
            // -----------------------------------------------------

            var bin = await _unitOfWork.Bins.GetByIdAsync(id);


            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }


            return Ok(bin);
        }


        // =========================================================
        // GET: api/bins/warehouse/{warehouseId}
        // =========================================================

        [HttpGet("warehouse/{warehouseId:int}")]
        public async Task<IActionResult> GetBinsByWarehouse(
            int warehouseId)
        {
            // -----------------------------------------------------
            // Validate WarehouseId
            // -----------------------------------------------------

            if (warehouseId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            // -----------------------------------------------------
            // Check Warehouse exists
            // -----------------------------------------------------

            var warehouse = await _unitOfWork.Warehouses
                .GetEntityByIdAsync(warehouseId);


            if (warehouse == null)
            {
                return NotFound(new
                {
                    message = "Warehouse not found."
                });
            }


            // -----------------------------------------------------
            // Get Bins
            // -----------------------------------------------------

            var bins = await _unitOfWork.Bins
                .GetByWarehouseIdAsync(warehouseId);


            return Ok(bins);
        }


        // =========================================================
        // GET: api/bins/partition/{partitionId}
        // =========================================================

        [HttpGet("partition/{partitionId:int}")]
        public async Task<IActionResult> GetBinsByPartition(
            int partitionId)
        {
            // -----------------------------------------------------
            // Validate PartitionId
            // -----------------------------------------------------

            if (partitionId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid PartitionId."
                });
            }


            // -----------------------------------------------------
            // Check Partition exists
            // -----------------------------------------------------

            var partition = await _unitOfWork.Partitions
                .GetEntityByIdAsync(partitionId);


            if (partition == null)
            {
                return NotFound(new
                {
                    message = "Partition not found."
                });
            }


            // -----------------------------------------------------
            // Get Bins
            // -----------------------------------------------------

            var bins = await _unitOfWork.Bins
                .GetByPartitionIdAsync(partitionId);


            return Ok(bins);
        }


        // =========================================================
        // GET: api/bins/location/{locationId}
        // =========================================================

        [HttpGet("location/{locationId:int}")]
        public async Task<IActionResult> GetBinsByLocation(
            int locationId)
        {
            // -----------------------------------------------------
            // Validate LocationId
            // -----------------------------------------------------

            if (locationId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid LocationId."
                });
            }


            // -----------------------------------------------------
            // Check Location exists
            // -----------------------------------------------------

            var location = await _unitOfWork.Locations
                .GetEntityByIdAsync(locationId);


            if (location == null)
            {
                return NotFound(new
                {
                    message = "Location not found."
                });
            }


            // -----------------------------------------------------
            // Get Bin
            // -----------------------------------------------------

            var bins = await _unitOfWork.Bins
                .GetByLocationIdAsync(locationId);


            return Ok(bins);
        }


        // =========================================================
        // POST: api/bins
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> CreateBin(
            [FromBody] CreateBinDto dto)
        {
            // -----------------------------------------------------
            // Validate ModelState
            // -----------------------------------------------------

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // -----------------------------------------------------
            // Validate WarehouseId
            // -----------------------------------------------------

            if (dto.WarehouseId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            // -----------------------------------------------------
            // Check Warehouse exists
            // -----------------------------------------------------

            var warehouse = await _unitOfWork.Warehouses
                .GetEntityByIdAsync(dto.WarehouseId);


            if (warehouse == null)
            {
                return BadRequest(new
                {
                    message = "Warehouse not found."
                });
            }


            // -----------------------------------------------------
            // Validate PartitionId
            // -----------------------------------------------------

            if (dto.PartitionId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid PartitionId."
                });
            }


            // -----------------------------------------------------
            // Check Partition exists
            // -----------------------------------------------------

            var partition = await _unitOfWork.Partitions
                .GetEntityByIdAsync(dto.PartitionId);


            if (partition == null)
            {
                return BadRequest(new
                {
                    message = "Partition not found."
                });
            }


            // -----------------------------------------------------
            // Make sure Partition belongs to Warehouse
            // -----------------------------------------------------

            if (partition.WarehouseId != dto.WarehouseId)
            {
                return BadRequest(new
                {
                    message =
                        "The selected partition does not belong to the selected warehouse."
                });
            }


            // -----------------------------------------------------
            // Validate Name
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Bin name is required."
                });
            }


            // -----------------------------------------------------
            // Validate Code
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest(new
                {
                    message = "Bin code is required."
                });
            }


            // -----------------------------------------------------
            // Create Bin
            // -----------------------------------------------------

            var bin = new Bin
            {
                PartitionId = dto.PartitionId,

                Bin_Code = dto.Code.Trim(),

                Bin_Name = dto.Name.Trim(),

                Bin_Description =
                    string.IsNullOrWhiteSpace(dto.Description)
                        ? null
                        : dto.Description.Trim(),

                IsActive = true
            };


            // -----------------------------------------------------
            // Add Bin
            // -----------------------------------------------------

            await _unitOfWork.Bins.AddAsync(bin);


            // -----------------------------------------------------
            // Save
            // -----------------------------------------------------

            await _unitOfWork.SaveAsync();


            // -----------------------------------------------------
            // Get created Bin
            // -----------------------------------------------------

            var result = await _unitOfWork.Bins
                .GetByIdAsync(bin.Bin_Id);


            return CreatedAtAction(
                nameof(GetBin),
                new
                {
                    id = bin.Bin_Id
                },
                result);
        }


        // =========================================================
        // PUT: api/bins/{id}
        // =========================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBin(
            int id,
            [FromBody] UpdateBinDto dto)
        {
            // -----------------------------------------------------
            // Validate ModelState
            // -----------------------------------------------------

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);


            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }


            // -----------------------------------------------------
            // Get Bin
            // -----------------------------------------------------

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(id);


            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }


            // =====================================================
            // Determine Partition
            // =====================================================

            var partitionId =
                dto.PartitionId ??
                bin.PartitionId;


            // -----------------------------------------------------
            // Validate PartitionId
            // -----------------------------------------------------

            if (partitionId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid PartitionId."
                });
            }


            // -----------------------------------------------------
            // Get Partition
            // -----------------------------------------------------

            var partition = await _unitOfWork.Partitions
                .GetEntityByIdAsync(partitionId);


            if (partition == null)
            {
                return BadRequest(new
                {
                    message = "Partition not found."
                });
            }


            // =====================================================
            // Determine Warehouse
            // =====================================================

            var warehouseId =
                dto.WarehouseId ??
                partition.WarehouseId;


            // -----------------------------------------------------
            // Validate WarehouseId
            // -----------------------------------------------------

            if (warehouseId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid WarehouseId."
                });
            }


            // -----------------------------------------------------
            // Check Warehouse exists
            // -----------------------------------------------------

            var warehouse = await _unitOfWork.Warehouses
                .GetEntityByIdAsync(warehouseId);


            if (warehouse == null)
            {
                return BadRequest(new
                {
                    message = "Warehouse not found."
                });
            }


            // -----------------------------------------------------
            // Make sure Partition belongs to Warehouse
            // -----------------------------------------------------

            if (partition.WarehouseId != warehouseId)
            {
                return BadRequest(new
                {
                    message =
                        "The selected partition does not belong to the selected warehouse."
                });
            }


            // =====================================================
            // Update Code
            // =====================================================

            if (dto.Code != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    return BadRequest(new
                    {
                        message = "Bin code cannot be empty."
                    });
                }

                bin.Bin_Code = dto.Code.Trim();
            }


            // =====================================================
            // Update Name
            // =====================================================

            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new
                    {
                        message = "Bin name cannot be empty."
                    });
                }

                bin.Bin_Name = dto.Name.Trim();
            }


            // =====================================================
            // Update Description
            // =====================================================

            if (dto.Description != null)
            {
                bin.Bin_Description =
                    string.IsNullOrWhiteSpace(dto.Description)
                        ? null
                        : dto.Description.Trim();
            }


            // =====================================================
            // Update Partition
            // =====================================================

            bin.PartitionId = partitionId;


            // =====================================================
            // Update Active Status
            // =====================================================

            if (dto.IsActive.HasValue)
            {
                bin.IsActive = dto.IsActive.Value;
            }


            // -----------------------------------------------------
            // IMPORTANT:
            // Bin does NOT have WarehouseId.
            //
            // Warehouse is determined through:
            // Bin -> Partition -> Warehouse
            // -----------------------------------------------------


            // =====================================================
            // Save
            // =====================================================

            _unitOfWork.Bins.Update(bin);

            await _unitOfWork.SaveAsync();


            // =====================================================
            // Return updated Bin
            // =====================================================

            var result = await _unitOfWork.Bins
                .GetByIdAsync(id);


            return Ok(result);
        }


        // =========================================================
        // DELETE: api/bins/{id}
        // =========================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBin(int id)
        {
            // -----------------------------------------------------
            // Validate Id
            // -----------------------------------------------------

            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid BinId."
                });
            }


            // -----------------------------------------------------
            // Get Bin
            // -----------------------------------------------------

            var bin = await _unitOfWork.Bins
                .GetEntityByIdAsync(id);


            if (bin == null)
            {
                return NotFound(new
                {
                    message = "Bin not found."
                });
            }


            // =====================================================
            // Prevent delete if Bin contains Stock
            // =====================================================

            if (bin.Stocks != null && bin.Stocks.Any())
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this bin because it contains stock."
                });
            }


            // =====================================================
            // Prevent delete if Bin contains Location
            // =====================================================

            if (bin.Location != null)
            {
                return Conflict(new
                {
                    message =
                        "Cannot delete this bin because it contains a location."
                });
            }


            // =====================================================
            // Delete Bin
            // =====================================================

            _unitOfWork.Bins.Delete(bin);


            // -----------------------------------------------------
            // Save
            // -----------------------------------------------------

            await _unitOfWork.SaveAsync();


            // =====================================================
            // Response
            // =====================================================

            return Ok(new
            {
                message = "Bin deleted successfully."
            });
        }
    }
}