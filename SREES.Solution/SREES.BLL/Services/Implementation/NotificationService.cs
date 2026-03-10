using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Constants;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Notifications;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IRealTimeNotificationService? _realTimeService;

        public NotificationService(
            ILogger<NotificationService> logger,
            IUnitOfWork uow,
            IMapper mapper,
            IRealTimeNotificationService? realTimeService = null)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _realTimeService = realTimeService;
        }

        public async Task<ResponsePackage<List<NotificationDataOut>>> GetNotificationsByUserId(int userId)
        {
            try
            {
                var notifications = await _uow.GetNotificationRepository().GetNotificationsByUserIdAsync(userId);
                var notificationList = _mapper.Map<List<NotificationDataOut>>(notifications.ToList());
                return new ResponsePackage<List<NotificationDataOut>>(notificationList, "Obave�tenja uspe�no preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju obave�tenja za korisnika {UserId}", userId);
                return new ResponsePackage<List<NotificationDataOut>>(null, "Gre�ka pri preuzimanju obave�tenja");
            }
        }

        public async Task<ResponsePackage<List<NotificationDataOut>>> GetUnreadNotificationsByUserId(int userId)
        {
            try
            {
                var notifications = await _uow.GetNotificationRepository().GetUnreadNotificationsByUserIdAsync(userId);
                var notificationList = _mapper.Map<List<NotificationDataOut>>(notifications.ToList());
                return new ResponsePackage<List<NotificationDataOut>>(notificationList, "Nepro?itana obave�tenja uspe�no preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju nepro?itanih obave�tenja za korisnika {UserId}", userId);
                return new ResponsePackage<List<NotificationDataOut>>(null, "Gre�ka pri preuzimanju nepro?itanih obave�tenja");
            }
        }

        public async Task<ResponsePackage<int>> GetUnreadCountByUserId(int userId)
        {
            try
            {
                var count = await _uow.GetNotificationRepository().GetUnreadCountByUserIdAsync(userId);
                return new ResponsePackage<int>(count, "Broj nepro?itanih obave�tenja uspe�no preuzet");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju broja nepro?itanih obave�tenja za korisnika {UserId}", userId);
                return new ResponsePackage<int>(0, "Gre�ka pri preuzimanju broja nepro?itanih obave�tenja");
            }
        }

        public async Task<ResponsePackage<NotificationDataOut?>> CreateNotification(NotificationDataIn notificationDataIn)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = notificationDataIn.UserId,
                    CustomerId = notificationDataIn.CustomerId,
                    OutageId = notificationDataIn.OutageId,
                    Title = notificationDataIn.Title,
                    Message = notificationDataIn.Message,
                    NotificationType = notificationDataIn.NotificationType,
                    IsRead = false,
                    Guid = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdateTime = DateTime.UtcNow
                };

                await _uow.GetNotificationRepository().AddAsync(notification);
                await _uow.CompleteAsync();

                var notificationDataOut = _mapper.Map<NotificationDataOut>(notification);

                // Šalji real-time notifikaciju preko SignalR
                if (_realTimeService != null)
                {
                    try
                    {
                        _logger.LogInformation("Šaljem real-time notifikaciju korisniku {UserId}...", notificationDataIn.UserId);
                        await _realTimeService.SendToUserAsync(notificationDataIn.UserId, notificationDataOut);
                        _logger.LogInformation("Real-time notifikacija uspešno poslata korisniku {UserId}", notificationDataIn.UserId);
                        
                        // Ažuriraj broj nepročitanih
                        var unreadCount = await _uow.GetNotificationRepository().GetUnreadCountByUserIdAsync(notificationDataIn.UserId);
                        await _realTimeService.UpdateUnreadCountAsync(notificationDataIn.UserId, unreadCount);
                        _logger.LogInformation("Ažuriran unread count za korisnika {UserId}: {Count}", notificationDataIn.UserId, unreadCount);
                    }
                    catch (Exception rtEx)
                    {
                        _logger.LogWarning(rtEx, "Neuspešno slanje real-time notifikacije korisniku {UserId}", notificationDataIn.UserId);
                    }
                }
                else
                {
                    _logger.LogWarning("IRealTimeNotificationService nije dostupan - notifikacija neće biti poslata u realnom vremenu");
                }

                return new ResponsePackage<NotificationDataOut?>(notificationDataOut, "Obave�tenje uspe�no kreirano");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri kreiranju obave�tenja");
                return new ResponsePackage<NotificationDataOut?>(null, "Gre�ka pri kreiranju obave�tenja");
            }
        }

        public async Task<ResponsePackage<bool>> MarkAsRead(int notificationId)
        {
            try
            {
                await _uow.GetNotificationRepository().MarkAsReadAsync(notificationId);
                await _uow.CompleteAsync();
                return new ResponsePackage<bool>(true, "Obave�tenje ozna?eno kao pro?itano");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri ozna?avanju obave�tenja {NotificationId} kao pro?itanog", notificationId);
                return new ResponsePackage<bool>(false, "Gre�ka pri ozna?avanju obave�tenja kao pro?itanog");
            }
        }

        public async Task<ResponsePackage<bool>> MarkAllAsReadByUserId(int userId)
        {
            try
            {
                await _uow.GetNotificationRepository().MarkAllAsReadByUserIdAsync(userId);
                await _uow.CompleteAsync();
                return new ResponsePackage<bool>(true, "Sva obave�tenja ozna?ena kao pro?itana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri ozna?avanju svih obave�tenja kao pro?itanih za korisnika {UserId}", userId);
                return new ResponsePackage<bool>(false, "Gre�ka pri ozna?avanju svih obave�tenja kao pro?itanih");
            }
        }

        public async Task<ResponsePackage<NotificationDataOut?>> CreateOutageStatusNotification(int outageId, int userId, string oldStatus, string newStatus)
        {
            try
            {
                var outage = await _uow.GetOutageRepository().GetByIdAsync(outageId);
                if (outage == null)
                    return new ResponsePackage<NotificationDataOut?>(null, "Kvar nije prona?en");

                var notificationType = newStatus == "Resolved"
                    ? NotificationType.OutageResolved
                    : NotificationType.OutageStatusChanged;

                var title = newStatus == "Resolved"
                    ? "Kvar je re�en"
                    : "Promena statusa kvara";

                var message = $"Status kvara #{outageId} je promenjen sa '{oldStatus}' na '{newStatus}'.";
                if (newStatus == "Resolved")
                {
                    message = $"Kvar #{outageId} je uspe�no re�en. Hvala na strpljenju.";
                }

                var notificationDataIn = new NotificationDataIn
                {
                    UserId = userId,
                    OutageId = outageId,
                    Title = title,
                    Message = message,
                    NotificationType = notificationType
                };

                return await CreateNotification(notificationDataIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri kreiranju obave�tenja o promeni statusa kvara {OutageId}", outageId);
                return new ResponsePackage<NotificationDataOut?>(null, "Gre�ka pri kreiranju obave�tenja o promeni statusa kvara");
            }
        }

        public async Task<ResponsePackage<int>> NotifyUsersInRegion(int regionId, int outageId, string message)
        {
            try
            {
                // Prona?i sve korisnike koji imaju prijave u tom regionu
                var outages = await _uow.GetOutageRepository().GetAllAsync();
                var userIds = outages
                    .Where(o => o.RegionId == regionId && !o.IsDeleted)
                    .Select(o => o.UserId)
                    .Distinct()
                    .ToList();

                int notificationCount = 0;

                foreach (var userId in userIds)
                {
                    var notificationDataIn = new NotificationDataIn
                    {
                        UserId = userId,
                        OutageId = outageId,
                        Title = "Obave�tenje o kvaru u va�em regionu",
                        Message = message,
                        NotificationType = NotificationType.SystemAlert
                    };

                    await CreateNotification(notificationDataIn);
                    notificationCount++;
                }

                return new ResponsePackage<int>(notificationCount, $"Poslato {notificationCount} obave�tenja korisnicima u regionu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri slanju obave�tenja korisnicima u regionu {RegionId}", regionId);
                return new ResponsePackage<int>(0, "Gre�ka pri slanju obave�tenja korisnicima u regionu");
            }
        }

        public async Task<ResponsePackage<string>> DeleteNotification(int notificationId)
        {
            try
            {
                var notification = await _uow.GetNotificationRepository().GetByIdAsync(notificationId);
                if (notification == null)
                    return new ResponsePackage<string>(null, "Obave�tenje nije prona?eno");

                _uow.GetNotificationRepository().RemoveEntity(notification);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>("Obave�tenje uspe�no obrisano", "Obave�tenje uspe�no obrisano");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri brisanju obave�tenja {NotificationId}", notificationId);
                return new ResponsePackage<string>(null, "Gre�ka pri brisanju obave�tenja");
            }
        }
    }
}
