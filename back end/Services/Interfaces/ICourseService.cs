using School_Project_API.DTO;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseDTO> GetCourseByIdAsync(int id);
        Task<PagedResponse<CourseDTO>> GetAllCourseAsync(int pageNumber , int pageSize );

        Task<CourseDTO> AddCourseAsync(CourseDTO DTO);

        Task<CourseDTO> UpdateCourseAsync(int id, CourseDTO DTO);
        Task<bool> DeleteCourseAsync(int id);
    }
}
