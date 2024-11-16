using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IArticleAttachmentService
    {
        IEnumerable<ArticleAttachmentViewModel> GetArticleAttachments(int articleId);
        ArticleAttachmentViewModel AddAttachment(IFormFile file, int articleId, int userId);
        void DeleteAttachment(int attachmentId);
    }
}