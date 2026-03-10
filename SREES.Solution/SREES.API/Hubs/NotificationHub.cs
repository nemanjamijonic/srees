using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SREES.Common.Models.Dtos.Notifications;

namespace SREES.API.Hubs
{
    /// <summary>
    /// SignalR Hub za real-time notifikacije
    /// </summary>
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Kada se korisnik poveže, dodaj ga u grupu sa njegovim userId
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("id")?.Value 
                ?? Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("Korisnik {UserId} povezan na NotificationHub, ConnectionId: {ConnectionId}", userId, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Kada se korisnik odjavi, ukloni ga iz grupe
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst("id")?.Value
                ?? Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("Korisnik {UserId} odjavljen sa NotificationHub", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Metoda koju klijent može pozvati za pridruživanje specifi?noj grupi (npr. region)
        /// </summary>
        public async Task JoinRegionGroup(int regionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"region_{regionId}");
            _logger.LogInformation("ConnectionId {ConnectionId} pridružen grupi region_{RegionId}", Context.ConnectionId, regionId);
        }

        /// <summary>
        /// Metoda za napuštanje grupe regiona
        /// </summary>
        public async Task LeaveRegionGroup(int regionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"region_{regionId}");
            _logger.LogInformation("ConnectionId {ConnectionId} napustio grupu region_{RegionId}", Context.ConnectionId, regionId);
        }
    }
}
