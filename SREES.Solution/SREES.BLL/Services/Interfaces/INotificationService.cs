using SREES.Common.Models;
using SREES.Common.Models.Dtos.Notifications;
using SREES.DAL.Models;

namespace SREES.BLL.Services.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Preuzima sva obaveštenja za korisnika
        /// </summary>
        Task<ResponsePackage<List<NotificationDataOut>>> GetNotificationsByUserId(int userId);

        /// <summary>
        /// Preuzima nepro?itana obaveštenja za korisnika
        /// </summary>
        Task<ResponsePackage<List<NotificationDataOut>>> GetUnreadNotificationsByUserId(int userId);

        /// <summary>
        /// Preuzima broj nepro?itanih obaveštenja za korisnika
        /// </summary>
        Task<ResponsePackage<int>> GetUnreadCountByUserId(int userId);

        /// <summary>
        /// Kreira novo obaveštenje
        /// </summary>
        Task<ResponsePackage<NotificationDataOut?>> CreateNotification(NotificationDataIn notificationDataIn);

        /// <summary>
        /// Ozna?ava obaveštenje kao pro?itano
        /// </summary>
        Task<ResponsePackage<bool>> MarkAsRead(int notificationId);

        /// <summary>
        /// Ozna?ava sva obaveštenja korisnika kao pro?itana
        /// </summary>
        Task<ResponsePackage<bool>> MarkAllAsReadByUserId(int userId);

        /// <summary>
        /// Kreira obaveštenje o promeni statusa kvara
        /// </summary>
        Task<ResponsePackage<NotificationDataOut?>> CreateOutageStatusNotification(int outageId, int userId, string oldStatus, string newStatus);

        /// <summary>
        /// Kreira obaveštenje za sve korisnike u regionu kada se prijavi kvar
        /// </summary>
        Task<ResponsePackage<int>> NotifyUsersInRegion(int regionId, int outageId, string message);

        /// <summary>
        /// Briše obaveštenje
        /// </summary>
        Task<ResponsePackage<string>> DeleteNotification(int notificationId);
    }
}
