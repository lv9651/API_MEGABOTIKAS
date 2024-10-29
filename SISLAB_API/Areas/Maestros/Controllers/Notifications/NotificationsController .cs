using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationsController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
    {
        if (notification == null || string.IsNullOrWhiteSpace(notification.Message))
        {
            return BadRequest("El mensaje de la notificación no puede estar vacío.");
        }

        await _notificationService.AddNotificationAsync(notification);
        return Ok(new { message = "Notificación agregada exitosamente." });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<NotificationMessage>>> GetNotificationsById(string id)
    {
        var notifications = await _notificationService.GetAllNotificationsAsync(id);

        if (notifications == null || !notifications.Any())
        {
            return NotFound();
        }

        return Ok(notifications);
    }

    [HttpPost("{userId}/read")]
    public async Task<IActionResult> MarkNotificationsAsRead(string userId, [FromBody] List<int> notificationIds)
    {
        if (notificationIds == null || !notificationIds.Any())
        {
            return BadRequest("No se proporcionaron IDs de notificaciones.");
        }

        var result = await _notificationService.MarkNotificationsAsReadAsync(userId, notificationIds);

        if (result)
        {
            return Ok(new { success = true });
        }
        else
        {
            return NotFound(new { error = "No se encontraron notificaciones para los IDs proporcionados." });
        }
    }
}