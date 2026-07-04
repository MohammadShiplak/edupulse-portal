using School_Project_API.DTO;

namespace School_Project_API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardStatsAsync();
    }
}
