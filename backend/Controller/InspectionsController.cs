using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using whm.DTOs.Inspections;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InspectionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/inspections
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inspections = await _unitOfWork.Inspections.GetAllAsync();

            var result = inspections.Select(i => new InspectionDto
            {
                InspectionId = i.InspectionId,
                ReceiptItemId = i.ReceiptItemId,
                InspectedBy = i.InspectedBy,
                InspectedAt = i.InspectedAt,
                InspectionStatus = i.InspectionStatus,
                Notes = i.Notes
            });

            return Ok(result);
        }

        // GET: api/inspections/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var inspection =
                await _unitOfWork.Inspections.GetByIdAsync(id);

            if (inspection == null)
            {
                return NotFound(new
                {
                    message = "Inspection not found."
                });
            }

            var result = new InspectionDto
            {
                InspectionId = inspection.InspectionId,
                ReceiptItemId = inspection.ReceiptItemId,
                InspectedBy = inspection.InspectedBy,
                InspectedAt = inspection.InspectedAt,
                InspectionStatus = inspection.InspectionStatus,
                Notes = inspection.Notes
            };

            return Ok(result);
        }

        // POST: api/inspections
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
     [FromBody] CreateInspectionDto dto)
        {
            // Get User ID from JWT
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Unable to identify the authenticated user."
                });
            }

            // Check if this ReceiptItem already has an Inspection
            var existingInspection =
                await _unitOfWork.Inspections
                    .GetByReceiptItemIdAsync(dto.ReceiptItemId);

            if (existingInspection != null)
            {
                return Conflict(new
                {
                    message = "This ReceiptItem already has an inspection.",
                    inspectionId = existingInspection.InspectionId,
                    status = existingInspection.InspectionStatus
                });
            }

            var inspection = new Inspection
            {
                ReceiptItemId = dto.ReceiptItemId,

                // Get InspectedBy from JWT
                InspectedBy = userId,

                // Keep InspectedAt from DTO
                InspectedAt = dto.InspectedAt ?? DateTimeOffset.UtcNow,

                InspectionStatus = InspectionStatus.Pending,
                Notes = dto.Notes
            };

            await _unitOfWork.Inspections.AddAsync(inspection);

            await _unitOfWork.SaveAsync();

            var result = new InspectionDto
            {
                InspectionId = inspection.InspectionId,
                ReceiptItemId = inspection.ReceiptItemId,
                InspectedBy = inspection.InspectedBy,
                InspectedAt = inspection.InspectedAt,
                InspectionStatus = inspection.InspectionStatus,
                Notes = inspection.Notes
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = inspection.InspectionId },
                result
            );
        }

        // PUT: api/inspections/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateInspectionDto dto)
        {
            var inspection =
                await _unitOfWork.Inspections.GetByIdAsync(id);

            if (inspection == null)
            {
                return NotFound(new
                {
                    message = "Inspection not found."
                });
            }

            // Prevent updating a cancelled inspection
            if (inspection.InspectionStatus == InspectionStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message = "Cancelled inspection cannot be updated."
                });
            }

            inspection.InspectionStatus = dto.InspectionStatus;
            inspection.Notes = dto.Notes;

            await _unitOfWork.Inspections.UpdateAsync(inspection);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Inspection updated successfully.",
                inspectionId = inspection.InspectionId,
                status = inspection.InspectionStatus
            });
        }

        // POST: api/inspections/{id}/accept
        [HttpPost("{id:int}/accept")]
        public async Task<IActionResult> Accept(int id)
        {
            var inspection =
                await _unitOfWork.Inspections.GetByIdAsync(id);

            if (inspection == null)
            {
                return NotFound(new
                {
                    message = "Inspection not found."
                });
            }

            if (inspection.InspectionStatus == InspectionStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message = "Cancelled inspection cannot be accepted."
                });
            }

            if (inspection.InspectionStatus == InspectionStatus.Passed)
            {
                return BadRequest(new
                {
                    message = "Inspection is already accepted."
                });
            }

            inspection.InspectionStatus = InspectionStatus.Passed;
            inspection.InspectedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.Inspections.UpdateAsync(inspection);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Inspection accepted successfully.",
                inspectionId = inspection.InspectionId,
                receiptItemId = inspection.ReceiptItemId,
                status = inspection.InspectionStatus
            });
        }

        // POST: api/inspections/{id}/quarantine
        [HttpPost("{id:int}/quarantine")]
        public async Task<IActionResult> Quarantine(int id)
        {
            var inspection =
                await _unitOfWork.Inspections.GetByIdAsync(id);

            if (inspection == null)
            {
                return NotFound(new
                {
                    message = "Inspection not found."
                });
            }

            if (inspection.InspectionStatus == InspectionStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message = "Cancelled inspection cannot be quarantined."
                });
            }

            if (inspection.InspectionStatus == InspectionStatus.Quarantined)
            {
                return BadRequest(new
                {
                    message = "Inspection is already quarantined."
                });
            }

            inspection.InspectionStatus = InspectionStatus.Quarantined;
            inspection.InspectedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.Inspections.UpdateAsync(inspection);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Inspection quarantined successfully.",
                inspectionId = inspection.InspectionId,
                receiptItemId = inspection.ReceiptItemId,
                status = inspection.InspectionStatus
            });
        }

        // POST: api/inspections/{id}/reject
        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var inspection =
                await _unitOfWork.Inspections.GetByIdAsync(id);

            if (inspection == null)
            {
                return NotFound(new
                {
                    message = "Inspection not found."
                });
            }

            if (inspection.InspectionStatus == InspectionStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message = "Cancelled inspection cannot be rejected."
                });
            }

            if (inspection.InspectionStatus == InspectionStatus.Failed)
            {
                return BadRequest(new
                {
                    message = "Inspection is already rejected."
                });
            }

            inspection.InspectionStatus = InspectionStatus.Failed;
            inspection.InspectedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.Inspections.UpdateAsync(inspection);

            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Inspection rejected successfully.",
                inspectionId = inspection.InspectionId,
                receiptItemId = inspection.ReceiptItemId,
                status = inspection.InspectionStatus
            });
        }
    }
}