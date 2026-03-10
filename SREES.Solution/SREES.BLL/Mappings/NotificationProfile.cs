using AutoMapper;
using SREES.Common.Models.Dtos.Notifications;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDataOut>();
            CreateMap<NotificationDataIn, Notification>();
        }
    }
}
