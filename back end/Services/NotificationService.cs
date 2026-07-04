using Microsoft.AspNetCore.SignalR;
using School_Project_API.Hubs;
using Microsoft.AspNetCore.Authorization;   // for [Authorize] attribute
          // for Hub base class, Groups, Clients
using System.Security.Claims;
using School_Project_API.Services.Interfaces;
namespace School_Project_API.Services
{
    public class NotificationService:INotificationService
    {

        private readonly IHubContext<NotificationHub> _hubContext;  

        public NotificationService(IHubContext<NotificationHub>hubContext)
        {
            _hubContext = hubContext;
        }

       public async Task SendToAllAsync(string message,string type="info")
        {


            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new 
            {
                message,
                type,
                timestamp = DateTime.UtcNow
            });




        }
           
        public async Task SendToRoleAsync(string role,string message,string type ="info")
        {
            await _hubContext.Clients.Group(role).SendAsync("ReceiveNotification", new
            {
                message,
                type,
                timestamp = DateTime.UtcNow
            }); 
        }


    }
}
