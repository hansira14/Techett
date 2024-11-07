using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class NotificationRepository : BaseRepository, INotificationRepository
{
    public NotificationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<Notification> GetAllNotifications()
    {
        return this.GetDbSet<Notification>();
    }
    public void AddNotification(Notification notification)
    {
        this.GetDbSet<Notification>().Add(notification);
        this.UnitOfWork.SaveChanges();
    }
    public Notification GetNotificationById(int id)
    {
        return this.GetDbSet<Notification>().Find(id);
    }   
    
}