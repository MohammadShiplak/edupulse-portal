using School_Project_API.DTO;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<TeacherDTO> GetTeacherByIdAsync(int id);
        Task<PagedResponse<TeacherDTO>> GetAllTeachersAsync(int pageNumber, int pageSize);

        Task<TeacherDTO> AddTeachersAsync(TeacherDTO studentDTO);

        Task<TeacherDTO> UpdateTeachersAsync(int id, TeacherDTO studentDTO);
        Task<bool> DeleteTeacherAsync(int id);
    }
}
