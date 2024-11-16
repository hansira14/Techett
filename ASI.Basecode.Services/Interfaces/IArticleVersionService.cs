namespace ASI.Basecode.Services.Interfaces;

using System.Collections.Generic;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;

public interface IArticleVersionService
{
    IEnumerable<ArticleVersionViewModel> GetArticleVersions(int articleId);
    void CreateArticleVersion(int articleId, string title, string content, int userId);
    ArticleVersion GetVersionById(int versionId);
    void RestoreVersion(int versionId, int userId);
}
