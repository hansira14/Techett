using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;

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
        return this.GetDbSet<ArticleVersion>().Find(versionId);
    }
    public void DeleteArticleVersion(ArticleVersion version)    
    {
        this.GetDbSet<ArticleVersion>().Remove(version);
        this.UnitOfWork.SaveChanges();
    }
}