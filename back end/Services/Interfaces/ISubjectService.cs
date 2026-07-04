using School_Project_API.DTO;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectDTO> GetSubjectByIdAsync(int id);
        Task<PagedResponse<SubjectDTO>> GetAllSubjectAsync(int pageNumber, int pageSize);

        Task<SubjectDTO> AddSubjectAsync(SubjectDTO subjectTO);

        Task<SubjectDTO> UpdateSubjectAsync(int id, SubjectDTO subjectDTO);
        Task<bool> DeleteSubjectAsync(int id);
    }
}
