using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace School_Project_API.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {

        private readonly ILogger<NotificationHub> _logger;  

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }   


        public override async Task OnConnectedAsync()
        {
            try
            {
              

                var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
           

                // Log all claims for debugging
                var claims = Context.User?.Claims?.ToList() ?? new List<Claim>();
          
             

                if (!string.IsNullOrEmpty(role))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, role);
              
                }
            

                await base.OnConnectedAsync();
             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync");  

                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
            
               
                var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
                if (!string.IsNullOrEmpty(role))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, role);
                }
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR in OnDisconnectedAsync: {ex.Message}");
                throw;
            }
        }
    }
}