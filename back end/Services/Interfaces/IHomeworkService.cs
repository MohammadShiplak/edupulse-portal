using School_Project_API.DTO;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public interface IHomeworkService
    {
        // READ operations — return HomeworkDTO (the read DTO)
        Task<HomeworkDTO?> GetHomeworkByIdAsync(int id);
        Task<PagedResponse<HomeworkDTO>> GetAllHomeworkAsync(int pageNumber, int pageSize);
        Task<List<HomeworkDTO>> GetHomeworkByTeacherAsync(int teacherId);
        Task<List<HomeworkDTO>> GetHomeworkByClassAsync(int classId);
        Task<bool> DeleteHomeworkAsync(int id);
        Task<HomeworkDTO> AddHomeworkAsync(HomeworkCreateDTO homeworkDTO);
        Task<HomeworkDTO?> UpdateHomeworkAsync(int id,HomeworkCreateDTO homeworkDTO);
        Task<bool> DeleteHomeworkFileAsync(int homeworkId); 
    }
}
