using Microsoft.EntityFrameworkCore;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public SreesContext Context
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public NotificationRepository(SreesContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await Context.Notifications
                .Where(n => !n.IsDeleted && n.UserId == userId)
                .Include(n => n.Outage)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId)
        {
            return await Context.Notifications
                .Where(n => !n.IsDeleted && n.UserId == userId && !n.IsRead)
                .Include(n => n.Outage)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountByUserIdAsync(int userId)
        {
            return await Context.Notifications
                .Where(n => !n.IsDeleted && n.UserId == userId && !n.IsRead)
                .CountAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await Context.Notifications.FindAsync(notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                notification.LastUpdateTime = DateTime.UtcNow;
            }
        }

        public async Task MarkAllAsReadByUserIdAsync(int userId)
        {
            var notifications = await Context.Notifications
                .Where(n => !n.IsDeleted && n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                notification.LastUpdateTime = DateTime.UtcNow;
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByOutageIdAsync(int outageId)
        {
            return await Context.Notifications
                .Where(n => !n.IsDeleted && n.OutageId == outageId)
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}
