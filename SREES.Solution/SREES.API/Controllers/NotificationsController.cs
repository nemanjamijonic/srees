using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Notifications;
using SREES.Services.Interfaces;

namespace SREES.API.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje obaveštenjima
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationApplicationService _notificationApplicationService;

        public NotificationsController(INotificationApplicationService notificationApplicationService)
        {
            _notificationApplicationService = notificationApplicationService;
        }

        /// <summary>
        /// Preuzimanje svih obaveštenja za korisnika
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ResponsePackage<List<NotificationDataOut>>>> GetNotificationsByUserId(int userId)
        {
            var result = await _notificationApplicationService.GetNotificationsByUserId(userId);
            return Ok(result);
        }

        /// <summary>
        /// Preuzimanje nepro?itanih obaveštenja za korisnika
        /// </summary>
        [HttpGet("user/{userId}/unread")]
        public async Task<ActionResult<ResponsePackage<List<NotificationDataOut>>>> GetUnreadNotificationsByUserId(int userId)
        {
            var result = await _notificationApplicationService.GetUnreadNotificationsByUserId(userId);
            return Ok(result);
        }

        /// <summary>
        /// Preuzimanje broja nepro?itanih obaveštenja za korisnika
        /// </summary>
        [HttpGet("user/{userId}/unread/count")]
        public async Task<ActionResult<ResponsePackage<int>>> GetUnreadCountByUserId(int userId)
        {
            var result = await _notificationApplicationService.GetUnreadCountByUserId(userId);
            return Ok(result);
        }

        /// <summary>
        /// Kreiranje novog obaveštenja (samo Admin)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponsePackage<NotificationDataOut?>>> CreateNotification([FromBody] NotificationDataIn notificationDataIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _notificationApplicationService.CreateNotification(notificationDataIn);
            if (result.Data == null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetNotificationsByUserId), new { userId = result.Data.UserId }, result);
        }

        /// <summary>
        /// Ozna?avanje obaveštenja kao pro?itanog
        /// </summary>
        [HttpPut("{notificationId}/read")]
        public async Task<ActionResult<ResponsePackage<bool>>> MarkAsRead(int notificationId)
        {
            var result = await _notificationApplicationService.MarkAsRead(notificationId);
            return Ok(result);
        }

        /// <summary>
        /// Ozna?avanje svih obaveštenja korisnika kao pro?itanih
        /// </summary>
        [HttpPut("user/{userId}/read-all")]
        public async Task<ActionResult<ResponsePackage<bool>>> MarkAllAsReadByUserId(int userId)
        {
            var result = await _notificationApplicationService.MarkAllAsReadByUserId(userId);
            return Ok(result);
        }

        /// <summary>
        /// Brisanje obaveštenja
        /// </summary>
        [HttpDelete("{notificationId}")]
        public async Task<ActionResult<ResponsePackage<string>>> DeleteNotification(int notificationId)
        {
            var result = await _notificationApplicationService.DeleteNotification(notificationId);
            if (result.Data == null && result.Message!.Contains("nije prona?eno"))
                return NotFound(result);

            return Ok(result);
        }
    }
}
