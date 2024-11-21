using ASI.Basecode.Data.Models;
using System.Linq;

namespace ASI.Basecode.Data.Interfaces;

public interface IArticleVersionRepository
{
    void AddArticleVersion(ArticleVersion version);
    IQueryable<ArticleVersion> GetArticleVersions(int articleId);
    ArticleVersion GetArticleVersionById(int versionId);
    void UpdateArticle(Article article);
}
