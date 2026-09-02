using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Sites;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/sites")]
    public class SitesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SitesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /api/sites
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sites = await _unitOfWork.Sites.GetAllAsync();

            return Ok(sites);
        }

        // GET: /api/sites/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var site = await _unitOfWork.Sites.GetByIdAsync(id);

            if (site == null)
            {
                return NotFound(new
                {
                    message = "Site not found."
                });
            }

            return Ok(site);
        }

        // POST: /api/sites
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] SiteCreateDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var codeExists =
                await _unitOfWork.Sites.CodeExistsAsync(dto.Code);

            if (codeExists)
            {
                return Conflict(new
                {
                    message = "A site with this code already exists."
                });
            }

            var now = DateTimeOffset.UtcNow;

            var site = new Site
            {
                Code = dto.Code.Trim(),
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim(),
                IsActive = dto.IsActive,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _unitOfWork.Sites.AddAsync(site);

            await _unitOfWork.SaveAsync();

            var result =
                await _unitOfWork.Sites.GetByIdAsync(site.SiteId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = site.SiteId },
                result
            );
        }

        // PUT: /api/sites/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] SiteUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var site =
                await _unitOfWork.Sites.GetEntityByIdAsync(id);

            if (site == null)
            {
                return NotFound(new
                {
                    message = "Site not found."
                });
            }

            var codeExists =
                await _unitOfWork.Sites
                    .CodeExistsAsync(dto.Code, id);

            if (codeExists)
            {
                return Conflict(new
                {
                    message = "A site with this code already exists."
                });
            }

            site.Code = dto.Code.Trim();
            site.Name = dto.Name.Trim();
            site.Description = dto.Description?.Trim();
            site.IsActive = dto.IsActive;
            site.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.Sites.Update(site);

            await _unitOfWork.SaveAsync();

            var result =
                await _unitOfWork.Sites.GetByIdAsync(id);

            return Ok(result);
        }

        // DELETE: /api/sites/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var site =
                await _unitOfWork.Sites.GetEntityByIdAsync(id);

            if (site == null)
            {
                return NotFound(new
                {
                    message = "Site not found."
                });
            }

            _unitOfWork.Sites.Delete(site);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // GET: /api/sites/{id}/warehouses
        [HttpGet("{id:int}/warehouses")]
        public async Task<IActionResult> GetWarehouses(int id)
        {
            var site =
                await _unitOfWork.Sites.GetByIdAsync(id);

            if (site == null)
            {
                return NotFound(new
                {
                    message = "Site not found."
                });
            }

            var warehouses =
                await _unitOfWork.Sites.GetWarehousesAsync(id);

            return Ok(warehouses);
        }

        // GET: /api/sites/{id}/inventory
        [HttpGet("{id:int}/inventory")]
        public async Task<IActionResult> GetInventory(int id)
        {
            var site =
                await _unitOfWork.Sites.GetByIdAsync(id);

            if (site == null)
            {
                return NotFound(new
                {
                    message = "Site not found."
                });
            }

            var inventory =
                await _unitOfWork.Sites.GetInventoryAsync(id);

            return Ok(inventory);
        }

        // GET: /api/sites/{id}/stats
        [HttpGet("{id:int}/stats")]
        public async Task<IActionResult> GetStats(int id)
        {
            var stats =
                await _unitOfWork.Sites.GetStatsAsync(id);

            if (stats == null)
            {
                return NotFound(new
                {
                    message = "Site not found."
                });
            }

            return Ok(stats);
        }
    }
}