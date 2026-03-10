using SREES.Common.Models.Dtos.Notifications;

namespace SREES.BLL.Services.Interfaces
{
    /// <summary>
    /// Interfejs za real-time slanje notifikacija (implementira se u API sloju)
    /// </summary>
    public interface IRealTimeNotificationService
    {
        /// <summary>
        /// Šalje notifikaciju specifi?nom korisniku u realnom vremenu
        /// </summary>
        Task SendToUserAsync(int userId, NotificationDataOut notification);

        /// <summary>
        /// Šalje notifikaciju svim korisnicima u regionu
        /// </summary>
        Task SendToRegionAsync(int regionId, NotificationDataOut notification);

        /// <summary>
        /// Ažurira broj nepro?itanih notifikacija za korisnika
        /// </summary>
        Task UpdateUnreadCountAsync(int userId, int count);
    }
}
