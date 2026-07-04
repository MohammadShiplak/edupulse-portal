using School_Project_API.DTO;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public interface IClassService
    {
        Task<ClassDTO> GetClassByIdAsync(int id);
        Task<PagedResponse<ClassDTO>> GetAllClassesAsync(int pageNumber, int pageSize);

        Task<ClassDTO> AddClassesAsync(ClassDTO classDTO);

        Task<ClassDTO> UpdateClassesAsync(int id, ClassDTO classDTO);
        Task<bool> DeleteClassAsync(int id);
    }
}
