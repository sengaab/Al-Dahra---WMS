using whm.DTOs.Dashboard;

namespace whm.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> GetDashboardAsync(
            int? siteId,
            int? departmentId,
            int? warehouseId,
            DateTimeOffset? fromDate,
            DateTimeOffset? toDate);
    }
}