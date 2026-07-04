using School_Project_API.DTO;

namespace School_Project_API.Services.Interfaces
{
    public interface IAuthService
    {
Task<string>LoginAsync(LoginDTO loginDTO);
        Task<bool>RegisterAsync(RegisterDTO registerDTO);
    }
}
