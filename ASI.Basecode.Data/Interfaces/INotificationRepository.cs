using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface INotificationRepository
{
    IQueryable<Notification> GetAllNotifications();
    void AddNotification(Notification notification);
    Notification GetNotificationById(int id);
}