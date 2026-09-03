using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET /api/dashboard
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetDashboard(
            [FromQuery] int? siteId,
            [FromQuery] int? departmentId,
            [FromQuery] int? warehouseId,
            [FromQuery] DateTimeOffset? fromDate,
            [FromQuery] DateTimeOffset? toDate)
        {
            if (fromDate.HasValue &&
                toDate.HasValue &&
                fromDate > toDate)
            {
                return BadRequest(new
                {
                    message =
                        "fromDate cannot be greater than toDate."
                });
            }


            var dashboard =
                await _unitOfWork.Dashboard.GetDashboardAsync(
                    siteId,
                    departmentId,
                    warehouseId,
                    fromDate,
                    toDate);


            return Ok(dashboard);
        }
    }
}
