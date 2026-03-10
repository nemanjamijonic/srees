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

        public NotificationService(ILogger<NotificationService> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<List<NotificationDataOut>>> GetNotificationsByUserId(int userId)
        {
            try
            {
                var notifications = await _uow.GetNotificationRepository().GetNotificationsByUserIdAsync(userId);
                var notificationList = _mapper.Map<List<NotificationDataOut>>(notifications.ToList());
                return new ResponsePackage<List<NotificationDataOut>>(notificationList, "Obaveštenja uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju obaveštenja za korisnika {UserId}", userId);
                return new ResponsePackage<List<NotificationDataOut>>(null, "Greška pri preuzimanju obaveštenja");
            }
        }

        public async Task<ResponsePackage<List<NotificationDataOut>>> GetUnreadNotificationsByUserId(int userId)
        {
            try
            {
                var notifications = await _uow.GetNotificationRepository().GetUnreadNotificationsByUserIdAsync(userId);
                var notificationList = _mapper.Map<List<NotificationDataOut>>(notifications.ToList());
                return new ResponsePackage<List<NotificationDataOut>>(notificationList, "Nepro?itana obaveštenja uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju nepro?itanih obaveštenja za korisnika {UserId}", userId);
                return new ResponsePackage<List<NotificationDataOut>>(null, "Greška pri preuzimanju nepro?itanih obaveštenja");
            }
        }

        public async Task<ResponsePackage<int>> GetUnreadCountByUserId(int userId)
        {
            try
            {
                var count = await _uow.GetNotificationRepository().GetUnreadCountByUserIdAsync(userId);
                return new ResponsePackage<int>(count, "Broj nepro?itanih obaveštenja uspešno preuzet");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju broja nepro?itanih obaveštenja za korisnika {UserId}", userId);
                return new ResponsePackage<int>(0, "Greška pri preuzimanju broja nepro?itanih obaveštenja");
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
                return new ResponsePackage<NotificationDataOut?>(notificationDataOut, "Obaveštenje uspešno kreirano");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju obaveštenja");
                return new ResponsePackage<NotificationDataOut?>(null, "Greška pri kreiranju obaveštenja");
            }
        }

        public async Task<ResponsePackage<bool>> MarkAsRead(int notificationId)
        {
            try
            {
                await _uow.GetNotificationRepository().MarkAsReadAsync(notificationId);
                await _uow.CompleteAsync();
                return new ResponsePackage<bool>(true, "Obaveštenje ozna?eno kao pro?itano");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ozna?avanju obaveštenja {NotificationId} kao pro?itanog", notificationId);
                return new ResponsePackage<bool>(false, "Greška pri ozna?avanju obaveštenja kao pro?itanog");
            }
        }

        public async Task<ResponsePackage<bool>> MarkAllAsReadByUserId(int userId)
        {
            try
            {
                await _uow.GetNotificationRepository().MarkAllAsReadByUserIdAsync(userId);
                await _uow.CompleteAsync();
                return new ResponsePackage<bool>(true, "Sva obaveštenja ozna?ena kao pro?itana");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ozna?avanju svih obaveštenja kao pro?itanih za korisnika {UserId}", userId);
                return new ResponsePackage<bool>(false, "Greška pri ozna?avanju svih obaveštenja kao pro?itanih");
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
                    ? "Kvar je rešen"
                    : "Promena statusa kvara";

                var message = $"Status kvara #{outageId} je promenjen sa '{oldStatus}' na '{newStatus}'.";
                if (newStatus == "Resolved")
                {
                    message = $"Kvar #{outageId} je uspešno rešen. Hvala na strpljenju.";
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
                _logger.LogError(ex, "Greška pri kreiranju obaveštenja o promeni statusa kvara {OutageId}", outageId);
                return new ResponsePackage<NotificationDataOut?>(null, "Greška pri kreiranju obaveštenja o promeni statusa kvara");
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
                        Title = "Obaveštenje o kvaru u vašem regionu",
                        Message = message,
                        NotificationType = NotificationType.SystemAlert
                    };

                    await CreateNotification(notificationDataIn);
                    notificationCount++;
                }

                return new ResponsePackage<int>(notificationCount, $"Poslato {notificationCount} obaveštenja korisnicima u regionu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri slanju obaveštenja korisnicima u regionu {RegionId}", regionId);
                return new ResponsePackage<int>(0, "Greška pri slanju obaveštenja korisnicima u regionu");
            }
        }

        public async Task<ResponsePackage<string>> DeleteNotification(int notificationId)
        {
            try
            {
                var notification = await _uow.GetNotificationRepository().GetByIdAsync(notificationId);
                if (notification == null)
                    return new ResponsePackage<string>(null, "Obaveštenje nije prona?eno");

                _uow.GetNotificationRepository().RemoveEntity(notification);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>("Obaveštenje uspešno obrisano", "Obaveštenje uspešno obrisano");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju obaveštenja {NotificationId}", notificationId);
                return new ResponsePackage<string>(null, "Greška pri brisanju obaveštenja");
            }
        }
    }
}
