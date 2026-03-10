using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// Preuzima sva obaveštenja za odre?enog korisnika
        /// </summary>
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);

        /// <summary>
        /// Preuzima nepro?itana obaveštenja za odre?enog korisnika
        /// </summary>
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId);

        /// <summary>
        /// Preuzima broj nepro?itanih obaveštenja za korisnika
        /// </summary>
        Task<int> GetUnreadCountByUserIdAsync(int userId);

        /// <summary>
        /// Ozna?ava obaveštenje kao pro?itano
        /// </summary>
        Task MarkAsReadAsync(int notificationId);

        /// <summary>
        /// Ozna?ava sva obaveštenja korisnika kao pro?itana
        /// </summary>
        Task MarkAllAsReadByUserIdAsync(int userId);

        /// <summary>
        /// Preuzima obaveštenja za odre?eni kvar
        /// </summary>
        Task<IEnumerable<Notification>> GetNotificationsByOutageIdAsync(int outageId);
    }
}
