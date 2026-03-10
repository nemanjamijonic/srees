using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Notifications;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class NotificationApplicationService : INotificationApplicationService
    {
        private readonly INotificationService _notificationService;

        public NotificationApplicationService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<ResponsePackage<List<NotificationDataOut>>> GetNotificationsByUserId(int userId)
        {
            return await _notificationService.GetNotificationsByUserId(userId);
        }

        public async Task<ResponsePackage<List<NotificationDataOut>>> GetUnreadNotificationsByUserId(int userId)
        {
            return await _notificationService.GetUnreadNotificationsByUserId(userId);
        }

        public async Task<ResponsePackage<int>> GetUnreadCountByUserId(int userId)
        {
            return await _notificationService.GetUnreadCountByUserId(userId);
        }

        public async Task<ResponsePackage<NotificationDataOut?>> CreateNotification(NotificationDataIn notificationDataIn)
        {
            return await _notificationService.CreateNotification(notificationDataIn);
        }

        public async Task<ResponsePackage<bool>> MarkAsRead(int notificationId)
        {
            return await _notificationService.MarkAsRead(notificationId);
        }

        public async Task<ResponsePackage<bool>> MarkAllAsReadByUserId(int userId)
        {
            return await _notificationService.MarkAllAsReadByUserId(userId);
        }

        public async Task<ResponsePackage<string>> DeleteNotification(int notificationId)
        {
            return await _notificationService.DeleteNotification(notificationId);
        }
    }
}
