using School_Project_API.DTO;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDTO> GetStudentByIdAsync(int id);
        Task<PagedResponse<StudentDTO>> GetAllStudentsAsync(int pageNumber, int pageSize);

        Task<StudentDTO> AddStudentAsync(StudentDTO studentDTO);

        Task<StudentDTO> UpdateStudentAsync(int id, StudentDTO studentDTO);
        Task<bool> DeleteStudentAsync(int id);
    }
}
