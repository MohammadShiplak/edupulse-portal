using School_Project_API.DTO;

namespace School_Project_API.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<string> SendMessageAsync(string messages);
    }
}
