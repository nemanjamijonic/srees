using SREES.Common.Models.Dtos.Notifications;

namespace SREES.API.Hubs
{
    /// <summary>
    /// Interfejs za slanje real-time notifikacija preko SignalR
    /// </summary>
    public interface INotificationHubService
    {
        /// <summary>
        /// Šalje notifikaciju specifi?nom korisniku
        /// </summary>
        Task SendNotificationToUser(int userId, NotificationDataOut notification);

        /// <summary>
        /// Šalje notifikaciju svim korisnicima u odre?enom regionu
        /// </summary>
        Task SendNotificationToRegion(int regionId, NotificationDataOut notification);

        /// <summary>
        /// Šalje notifikaciju svim povezanim korisnicima
        /// </summary>
        Task SendNotificationToAll(NotificationDataOut notification);

        /// <summary>
        /// Šalje broj nepro?itanih notifikacija korisniku
        /// </summary>
        Task SendUnreadCountToUser(int userId, int unreadCount);
    }
}
