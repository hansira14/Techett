using ASI.Basecode.Data.Models;
using System.Linq;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IAttachmentRepository
    {
        IQueryable<Attachment> GetAllAttachments();
        Attachment GetAttachmentById(int id);
        void AddAttachment(Attachment attachment);
        void DeleteAttachment(Attachment attachment);
        IQueryable<Attachment> GetTicketAttachments(int ticketId);
    }
}