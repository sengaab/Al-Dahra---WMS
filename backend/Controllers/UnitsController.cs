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
    public class UnitsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UnitsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // =========================================
        // GET ALL
        // GET: api/Units
        // =========================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var units =
                await unitOfWork.Units.GetAllAsync();

            return Ok(units.Select(u => new
            {
                unitId = u.Unit_Id,
                unitName = u.Unit_Name,
                unitSymbol = u.Unit_Symbol,
                isActive = u.IsActive
            }));
        }

        // =========================================
        // GET BY ID
        // GET: api/Units/1
        // =========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var unit =
                await unitOfWork.Units.GetByIdAsync(id);

            if (unit == null)
            {
                return NotFound("Unit not found.");
            }

            return Ok(new
            {
                unitId = unit.Unit_Id,
                unitName = unit.Unit_Name,
                unitSymbol = unit.Unit_Symbol,
                isActive = unit.IsActive
            });
        }

        // =========================================
        // CREATE
        // POST: api/Units
        // =========================================

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateUnitDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing =
                await unitOfWork.Units
                    .GetByNameAsync(dto.Unit_Name.Trim());

            if (existing != null)
            {
                return Conflict(
                    "Unit with this name already exists.");
            }

            var unit = new Unit
            {
                Unit_Name = dto.Unit_Name.Trim(),

                Unit_Symbol = dto.Unit_Symbol.Trim(),

                IsActive = true
            };

            await unitOfWork.Units.AddAsync(unit);

            await unitOfWork.SaveAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = unit.Unit_Id },
                new
                {
                    message = "Unit created successfully.",
                    unitId = unit.Unit_Id,
                    unitName = unit.Unit_Name,
                    unitSymbol = unit.Unit_Symbol
                });
        }

        // =========================================
        // UPDATE
        // PUT: api/Units/1
        // =========================================

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateUnitDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unit =
                await unitOfWork.Units.GetByIdAsync(id);

            if (unit == null)
            {
                return NotFound("Unit not found.");
            }

            var existing =
                await unitOfWork.Units
                    .GetByNameAsync(dto.Unit_Name.Trim());

            if (existing != null &&
                existing.Unit_Id != id)
            {
                return Conflict(
                    "Another unit with this name already exists.");
            }

            unit.Unit_Name =
                dto.Unit_Name.Trim();

            unit.Unit_Symbol =
                dto.Unit_Symbol.Trim();

            unit.IsActive =
                dto.IsActive;

            unitOfWork.Units.Update(unit);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Unit updated successfully."
            });
        }

        // =========================================
        // DELETE
        // DELETE: api/Units/1
        // =========================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var unit =
                await unitOfWork.Units.GetByIdAsync(id);

            if (unit == null)
            {
                return NotFound("Unit not found.");
            }

            // Soft Delete
            unit.IsActive = false;

            unitOfWork.Units.Update(unit);

            await unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Unit deactivated successfully."
            });
        }
    }
}