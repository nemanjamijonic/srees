using Microsoft.AspNetCore.SignalR;
using SREES.Common.Models.Dtos.Notifications;

namespace SREES.API.Hubs
{
    /// <summary>
    /// Servis za slanje real-time notifikacija preko SignalR
    /// </summary>
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationHubService> _logger;

        public NotificationHubService(IHubContext<NotificationHub> hubContext, ILogger<NotificationHubService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendNotificationToUser(int userId, NotificationDataOut notification)
        {
            try
            {
                await _hubContext.Clients.Group($"user_{userId}").SendAsync("ReceiveNotification", notification);
                _logger.LogInformation("Poslata notifikacija korisniku {UserId}: {Title}", userId, notification.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri slanju notifikacije korisniku {UserId}", userId);
            }
        }

        public async Task SendNotificationToRegion(int regionId, NotificationDataOut notification)
        {
            try
            {
                await _hubContext.Clients.Group($"region_{regionId}").SendAsync("ReceiveNotification", notification);
                _logger.LogInformation("Poslata notifikacija regionu {RegionId}: {Title}", regionId, notification.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri slanju notifikacije regionu {RegionId}", regionId);
            }
        }

        public async Task SendNotificationToAll(NotificationDataOut notification)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
                _logger.LogInformation("Poslata notifikacija svim korisnicima: {Title}", notification.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri slanju notifikacije svim korisnicima");
            }
        }

        public async Task SendUnreadCountToUser(int userId, int unreadCount)
        {
            try
            {
                await _hubContext.Clients.Group($"user_{userId}").SendAsync("UpdateUnreadCount", unreadCount);
                _logger.LogInformation("Poslat broj nepro?itanih ({Count}) korisniku {UserId}", unreadCount, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri slanju broja nepro?itanih korisniku {UserId}", userId);
            }
        }
    }
}
