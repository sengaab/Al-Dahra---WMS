using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Site;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public SiteController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =====================================================
        // 1. GET ALL
        // GET: api/Site
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var sites =
                await unitOfWork.Sites
                    .GetAllAsync();

            var result =
                sites.Select(s => new
                {
                    siteId =
                        s.Site_Id,

                    siteName =
                        s.Site_Name,

                    siteCode =
                        s.Site_Code,

                    siteDescription =
                        s.Site_Description,

                    isActive =
                        s.IsActive
                });

            return Ok(result);
        }


        // =====================================================
        // 2. GET BY ID
        // GET: api/Site/1
        // =====================================================

        [HttpGet("GetbyId/{id:int}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var site =
                await unitOfWork.Sites
                    .GetByIdAsync(id);

            if (site == null)
            {
                return NotFound(
                    "Site not found.");
            }

            return Ok(new
            {
                siteId =
                    site.Site_Id,

                siteName =
                    site.Site_Name,

                siteCode =
                    site.Site_Code,

                siteDescription =
                    site.Site_Description,

                isActive =
                    site.IsActive
            });
        }


        // =====================================================
        // 3. GET BY CODE
        // GET: api/Site/GetbyCode/{code}
        // =====================================================

        [HttpGet("GetbyCode/{code}")]
        public async Task<IActionResult> GetByCode(
            string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest(
                    "Site code is required.");
            }

            var site =
                await unitOfWork.Sites
                    .GetByCodeAsync(code.Trim());

            if (site == null)
            {
                return NotFound(
                    "Site not found.");
            }

            return Ok(new
            {
                siteId =
                    site.Site_Id,

                siteName =
                    site.Site_Name,

                siteCode =
                    site.Site_Code,

                siteDescription =
                    site.Site_Description,

                isActive =
                    site.IsActive
            });
        }


        // =====================================================
        // 4. CREATE
        // POST: api/Site
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            CreateSiteDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // CHECK CODE
            // =================================================

            if (!string.IsNullOrWhiteSpace(dto.Site_Code))
            {
                var existingSite =
                    await unitOfWork.Sites
                        .GetByCodeAsync(
                            dto.Site_Code.Trim());

                if (existingSite != null)
                {
                    return BadRequest(
                        "Site code already exists.");
                }
            }


            // =================================================
            // CREATE SITE
            // =================================================

            var site =
                new Site
                {
                    Site_Name =
                        dto.Site_Name.Trim(),

                    Site_Code =
                        string.IsNullOrWhiteSpace(
                            dto.Site_Code)
                            ? null
                            : dto.Site_Code.Trim(),

                    Site_Description =
                        string.IsNullOrWhiteSpace(
                            dto.Site_Description)
                            ? null
                            : dto.Site_Description.Trim(),

                    IsActive =
                        true
                };


            await unitOfWork.Sites
                .AddAsync(site);

            await unitOfWork
                .SaveAsync();


            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = site.Site_Id
                },
                new
                {
                    message =
                        "Site created successfully.",

                    siteId =
                        site.Site_Id,

                    siteName =
                        site.Site_Name,

                    siteCode =
                        site.Site_Code,

                    siteDescription =
                        site.Site_Description,

                    isActive =
                        site.IsActive
                });
        }


        // =====================================================
        // 5. UPDATE
        // PUT: api/Site/1
        // =====================================================

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateSiteDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var site =
                await unitOfWork.Sites
                    .GetByIdAsync(id);

            if (site == null)
            {
                return NotFound(
                    "Site not found.");
            }


            // =================================================
            // CHECK CODE
            // =================================================

            if (!string.IsNullOrWhiteSpace(
                dto.Site_Code))
            {
                var existingSite =
                    await unitOfWork.Sites
                        .GetByCodeAsync(
                            dto.Site_Code.Trim());

                if (existingSite != null &&
                    existingSite.Site_Id != id)
                {
                    return BadRequest(
                        "Site code already exists.");
                }
            }


            // =================================================
            // UPDATE
            // =================================================

            site.Site_Name =
                dto.Site_Name.Trim();

            site.Site_Code =
                string.IsNullOrWhiteSpace(
                    dto.Site_Code)
                    ? null
                    : dto.Site_Code.Trim();

            site.Site_Description =
                string.IsNullOrWhiteSpace(
                    dto.Site_Description)
                    ? null
                    : dto.Site_Description.Trim();

            site.IsActive =
                dto.IsActive;


            unitOfWork.Sites
                .Update(site);

            await unitOfWork
                .SaveAsync();


            return Ok(new
            {
                message =
                    "Site updated successfully.",

                siteId =
                    site.Site_Id,

                siteName =
                    site.Site_Name,

                siteCode =
                    site.Site_Code,

                siteDescription =
                    site.Site_Description,

                isActive =
                    site.IsActive
            });
        }


        // =====================================================
        // 6. DELETE
        // DELETE: api/Site/1
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var site =
                await unitOfWork.Sites
                    .GetByIdAsync(id);

            if (site == null)
            {
                return NotFound(
                    "Site not found.");
            }


            // =================================================
            // SOFT DELETE
            // =================================================

            site.IsActive =
                false;


            unitOfWork.Sites
                .Update(site);

            await unitOfWork
                .SaveAsync();


            return Ok(new
            {
                message =
                    "Site deleted successfully.",

                siteId =
                    site.Site_Id,

                isActive =
                    site.IsActive
            });
        }
    }
}