using School_Project_API.DTO;

namespace School_Project_API.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<AttendanceDTO>> GetAllAttendancesAsync(); 
        Task<List<AttendanceDTO>>GetAttendanceByDateAsync(DateTime date);
        Task<List<AttendanceDTO>> GetAttendanceByStudentAsync(int Studentid);
        Task<AttendanceDTO?> GetAttendanceByIdAsync(int id);
        Task<AttendanceDTO?>AddAttendanceAsync(AttendanceDTO attendanceDTO);
        Task<AttendanceDTO?> UpdateAttendanceAsync(int id,AttendanceDTO attendanceDTO);
        Task<bool>DeleteAttendanceAsync(int id);    
    }
}
