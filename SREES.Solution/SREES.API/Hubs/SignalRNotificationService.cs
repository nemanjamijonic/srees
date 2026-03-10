using SREES.BLL.Services.Interfaces;
using SREES.Common.Models.Dtos.Notifications;

namespace SREES.API.Hubs
{
    /// <summary>
    /// Implementacija IRealTimeNotificationService koja koristi SignalR
    /// </summary>
    public class SignalRNotificationService : IRealTimeNotificationService
    {
        private readonly INotificationHubService _hubService;

        public SignalRNotificationService(INotificationHubService hubService)
        {
            _hubService = hubService;
        }

        public async Task SendToUserAsync(int userId, NotificationDataOut notification)
        {
            await _hubService.SendNotificationToUser(userId, notification);
        }

        public async Task SendToRegionAsync(int regionId, NotificationDataOut notification)
        {
            await _hubService.SendNotificationToRegion(regionId, notification);
        }

        public async Task UpdateUnreadCountAsync(int userId, int count)
        {
            await _hubService.SendUnreadCountToUser(userId, count);
        }
    }
}
