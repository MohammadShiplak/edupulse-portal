using School_Project_API.DTO;

namespace School_Project_API.Data.Config
{
    public interface IProgressService
    {

        Task<CourseProgressDTO> CalculateAndSaveAsync(int studentId, int courseId);
        Task<CourseProgressDTO> GetProgressAsync(int studentId, int courseId);

        Task<List<CourseProgressDTO>> GetStudentProgressAsync(int studentId);

        Task<List<CourseProgressDTO>> GetCourseProgressAsync(int studentId);








    }
}
