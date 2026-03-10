using SREES.Common.Constants;

namespace SREES.Common.Models.Dtos.Notifications
{
    public class NotificationDataIn
    {
        public int UserId { get; set; }
        public int? CustomerId { get; set; }
        public int? OutageId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType NotificationType { get; set; }
    }
}
