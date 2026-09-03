using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Unit;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL
        // GET /api/units
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            var units =
                await _unitOfWork.Units.GetAllAsync();

            return Ok(units);
        }


        // =====================================================
        // GET BY ID
        // GET /api/units/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUnit(int id)
        {
            var unit =
                await _unitOfWork.Units.GetByIdAsync(id);

            if (unit == null)
            {
                return NotFound(new
                {
                    message = "Unit not found."
                });
            }

            return Ok(unit);
        }


        // =====================================================
        // CREATE
        // POST /api/units
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateUnit(
            [FromBody] CreateUnitDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Unit name is required."
                });
            }


            if (string.IsNullOrWhiteSpace(dto.Abbreviation))
            {
                return BadRequest(new
                {
                    message = "Unit abbreviation is required."
                });
            }


            var unit = new Unit
            {
                Name = dto.Name.Trim(),

                Abbreviation = dto.Abbreviation.Trim(),

                CreatedAt = DateTimeOffset.UtcNow,

                UpdatedAt = DateTimeOffset.UtcNow
            };


            await _unitOfWork.Units
                .AddAsync(unit);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Units
                    .GetByIdAsync(unit.UnitId);


            return CreatedAtAction(
                nameof(GetUnit),
                new
                {
                    id = unit.UnitId
                },
                result);
        }


        // =====================================================
        // UPDATE
        // PUT /api/units/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUnit(
            int id,
            [FromBody] UpdateUnitDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }


            var unit =
                await _unitOfWork.Units
                    .GetEntityByIdAsync(id);

            if (unit == null)
            {
                return NotFound(new
                {
                    message = "Unit not found."
                });
            }


            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Unit name is required."
                });
            }


            if (string.IsNullOrWhiteSpace(dto.Abbreviation))
            {
                return BadRequest(new
                {
                    message = "Unit abbreviation is required."
                });
            }


            unit.Name = dto.Name.Trim();

            unit.Abbreviation = dto.Abbreviation.Trim();

            unit.UpdatedAt = DateTimeOffset.UtcNow;


            _unitOfWork.Units
                .Update(unit);

            await _unitOfWork.SaveAsync();


            var result =
                await _unitOfWork.Units
                    .GetByIdAsync(id);

            return Ok(result);
        }


        // =====================================================
        // DELETE
        // DELETE /api/units/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var unit =
                await _unitOfWork.Units
                    .GetEntityByIdAsync(id);

            if (unit == null)
            {
                return NotFound(new
                {
                    message = "Unit not found."
                });
            }


            _unitOfWork.Units
                .Delete(unit);

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message = "Unit deleted successfully."
            });
        }
    }
}