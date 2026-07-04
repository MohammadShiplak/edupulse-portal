using School_Project_API.DTO;

namespace School_Project_API.Services.Interfaces
{
    public interface IAttendanceAlertService
    {
        Task CheckAndCreateAlertAsync(int studentId, DateTime date);

        Task<List<AttendanceAlertDTO>> GetAllAlertsAsync();

        Task<List<AttendanceAlertDTO>> GetActiveAlertsAsync();

        Task<List<AttendanceAlertDTO>> GetAlertsByStudentAsync(int studentId);

        Task<AttendanceAlertDTO?> ResolveAlertAsync(int alertId, ResolveAlertDTO resolveDTO);

        Task<int> GetActiveAlertCountAsync();   
    }
}
