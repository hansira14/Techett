using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data.Repositories;

public class ArticleAttachmentRepository : BaseRepository, IArticleAttachmentRepository
{
    public ArticleAttachmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<ArticleAttachment> GetAllArticleAttachments() 
    {
        return this.GetDbSet<ArticleAttachment>()
            .Include(a => a.UploadedByNavigation);
    }

    public ArticleAttachment GetArticleAttachmentById(int id)   
    {
        return this.GetDbSet<ArticleAttachment>().Find(id);
    }

    public void AddArticleAttachment(ArticleAttachment articleAttachment)
    {
        this.GetDbSet<ArticleAttachment>().Add(articleAttachment);
        this.UnitOfWork.SaveChanges();
    }

    public void DeleteArticleAttachment(ArticleAttachment articleAttachment)
    {
        this.GetDbSet<ArticleAttachment>().Remove(articleAttachment);
        this.UnitOfWork.SaveChanges();
    }

    public IQueryable<ArticleAttachment> GetArticleAttachmentsByArticleId(int articleId)
    {
        return this.GetDbSet<ArticleAttachment>()
            .Include(a => a.UploadedByNavigation)
            .Where(a => a.ArticleId == articleId);
    }
}