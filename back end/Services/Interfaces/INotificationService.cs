namespace School_Project_API.Services.Interfaces
{
    public interface INotificationService
    {

        Task SendToAllAsync(string message,string type="info");

        Task SendToRoleAsync(string role, string message, string type = "info");


    }
}
