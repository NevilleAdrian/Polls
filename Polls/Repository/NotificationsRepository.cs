using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polls.Data;
using Polls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polls.Repository
{
    public class NotificationsRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger<NotificationsRepository> _logger;
        public NotificationsRepository(ApplicationDbContext context,
            ILogger<NotificationsRepository> logger)
        {
            _ctx = context;
            _logger = logger;
        }

        public async Task AddNotification(string message, string userId)
        {
            Notification notification = new Notification
            {
                HasBeenSeen = false,
                Message = message,
                UserId = userId
            };
            try
            {
                await _ctx.Notifications.AddAsync(notification);
                await _ctx.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There was an error {ex.Message}");
            }
        }

        public async Task<Notification> GetNotificationById(int id, bool isSeeing = false)
        {
            if (NotificationExists(id))
            {
                Notification notification = await _ctx.Notifications.Include(n => n.User).SingleOrDefaultAsync(n => n.NotificationId == id);
                if (isSeeing)
                {
                    notification.HasBeenSeen = true;
                    await UpdateNotificationById(notification);
                }
                return notification;

            }
            return null;
        }

        public async Task<List<Notification>> GetAllNotifications()
        {

            return await _ctx.Notifications.Include(n => n.User).ToListAsync();
        }

        public async Task<bool> UpdateNotificationById(Notification notification)
        {
            if (NotificationExists(notification.NotificationId))
            {
                try
                {
                    _ctx.Notifications.Update(notification);
                    await _ctx.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"There was an error {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> DeleteNotification(int id)
        {
            if (NotificationExists(id))
            {
                try
                {
                    Notification notification = await _ctx.Notifications.FindAsync(id);
                    _ctx.Notifications.Remove(notification);
                    await _ctx.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"There was an error {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public bool NotificationExists(int id) => _ctx.Notifications.Any(n => n.NotificationId == id);
    }
}
