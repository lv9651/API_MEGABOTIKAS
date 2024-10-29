using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SISLAB_API.Areas.Maestros.Services
{
    public class NotificationService
    {
        private readonly NotificationRepository _notificationRepository;

        public NotificationService(NotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task<IEnumerable<NotificationMessage>> GetAllNotificationsAsync(string id)
        {
            return await _notificationRepository.GetNotificationsByIdAsync(id);
        }

        public async Task<bool> MarkNotificationsAsReadAsync(string userId, List<int> notificationIds)
        {
            return await _notificationRepository.MarkNotificationsAsReadAsync(userId, notificationIds);
        }
    }
}