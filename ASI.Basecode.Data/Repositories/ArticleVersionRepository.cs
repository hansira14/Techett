using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data.Repositories;

public class ArticleVersionRepository : BaseRepository, IArticleVersionRepository
{
    public ArticleVersionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public void AddArticleVersion(ArticleVersion version)
    {
        this.GetDbSet<ArticleVersion>().Add(version);
        this.UnitOfWork.SaveChanges();
    }
    public IQueryable<ArticleVersion> GetArticleVersions(int articleId)
    {
        return this.GetDbSet<ArticleVersion>()
                  .Where(v => v.ArticleId == articleId)
                  .OrderByDescending(v => v.VersionDate);
    }
    public ArticleVersion GetArticleVersionById(int versionId)
    {
        return this.GetDbSet<ArticleVersion>()
            .Include(v => v.VersionedByNavigation)
            .Include(v => v.Article)
            .FirstOrDefault(v => v.VersionId == versionId);
    }
    public void UpdateArticle(Article article)
    {
        this.GetDbSet<Article>().Update(article);
        this.UnitOfWork.SaveChanges();
    }
}
