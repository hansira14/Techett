using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface IArticleAttachmentRepository
{
    IQueryable<ArticleAttachment> GetAllArticleAttachments();
    ArticleAttachment GetArticleAttachmentById(int id);
    void AddArticleAttachment(ArticleAttachment articleAttachment);
    void DeleteArticleAttachment(ArticleAttachment articleAttachment);
    IQueryable<ArticleAttachment> GetArticleAttachmentsByArticleId(int articleId);  
}