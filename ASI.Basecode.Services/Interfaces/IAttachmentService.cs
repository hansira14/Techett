using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IAttachmentService
    {
        IEnumerable<AttachmentViewModel> GetTicketAttachments(int ticketId);
        AttachmentViewModel AddAttachment(IFormFile file, int ticketId, int userId);
        void DeleteAttachment(int attachmentId);
        AttachmentViewModel GetAttachmentById(int attachmentId);
    }
}