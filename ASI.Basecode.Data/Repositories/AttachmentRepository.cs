using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASI.Basecode.Data.Repositories;

public class AttachmentRepository : BaseRepository, IAttachmentRepository
{
    public AttachmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public IQueryable<Attachment> GetAllAttachments()
    {
        return this.GetDbSet<Attachment>()
            .Include(a => a.UploadedByNavigation);
    }

    public Attachment GetAttachmentById(int id)
    {
        return this.GetDbSet<Attachment>().Find(id);
    }

    public void AddAttachment(Attachment attachment)
    {
        this.GetDbSet<Attachment>().Add(attachment);
        this.UnitOfWork.SaveChanges();
    }

    public void DeleteAttachment(Attachment attachment)
    {
        this.GetDbSet<Attachment>().Remove(attachment);
        this.UnitOfWork.SaveChanges();
    }

    public IQueryable<Attachment> GetTicketAttachments(int ticketId)
    {
        return this.GetDbSet<Attachment>()
            .Include(a => a.UploadedByNavigation)
            .Where(a => a.TicketId == ticketId);
    }
}