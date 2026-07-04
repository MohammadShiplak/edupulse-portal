using School_Project_API.DTO;

namespace School_Project_API.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<DepartmentDTO> GetDepartmentByIdAsync(int ?id);
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();

        Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO departmentDTO);

        Task<DepartmentDTO> UpdateDepartmentAsync(int id, DepartmentDTO departmentDTO);
        Task<bool> DeleteDepartemntAsync(int id);

        Task<DepartmentStatisticsDTO> GetDepartmentStatisticsAsync(int id);   
    }
}
