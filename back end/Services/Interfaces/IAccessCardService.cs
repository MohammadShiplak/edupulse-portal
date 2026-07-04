using School_Project_API.DTO;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public interface IAccessCardService
    {
        Task<AccessCardDTO> GetAccessCardByIdAsync(int id);
        Task<PagedResponse<AccessCardDTO>> GetAllCardsAsync(int pageNumber, int pageSize);

        Task<AccessCardDTO> AddAccessCardAsync(AccessCardDTO cardDTO);

        Task<AccessCardDTO> UpdateAccessAsync(int id, AccessCardDTO cardDTO);
        Task<bool> DeleteAccessAsync(int id);
    }
}
