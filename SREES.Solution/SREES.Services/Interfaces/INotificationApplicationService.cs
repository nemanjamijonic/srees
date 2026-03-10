using SREES.Common.Models;
using SREES.Common.Models.Dtos.Notifications;

namespace SREES.Services.Interfaces
{
    public interface INotificationApplicationService
    {
        Task<ResponsePackage<List<NotificationDataOut>>> GetNotificationsByUserId(int userId);
        Task<ResponsePackage<List<NotificationDataOut>>> GetUnreadNotificationsByUserId(int userId);
        Task<ResponsePackage<int>> GetUnreadCountByUserId(int userId);
        Task<ResponsePackage<NotificationDataOut?>> CreateNotification(NotificationDataIn notificationDataIn);
        Task<ResponsePackage<bool>> MarkAsRead(int notificationId);
        Task<ResponsePackage<bool>> MarkAllAsReadByUserId(int userId);
        Task<ResponsePackage<string>> DeleteNotification(int notificationId);
    }
}
