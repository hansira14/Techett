namespace ASI.Basecode.Services.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using ASI.Basecode.Services.ServiceModels;

public interface INotificationService
{
    Task CreateNotification(int ticketId, string message, string type, int userId, int agentId);
    IEnumerable<NotificationViewModel> GetUserNotifications(int userId);
    void MarkAsRead(int notificationId);
    Task<bool> SendEmailNotification(string recipientEmail, string subject, string message);
}