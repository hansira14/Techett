namespace ASI.Basecode.Services.Interfaces;

using System.Collections.Generic;
using ASI.Basecode.Services.ServiceModels;

public interface IArticleVersionService
{
    IEnumerable<ArticleVersionViewModel> GetArticleVersions(int articleId);
    void CreateArticleVersion(int articleId, string title, string content, int userId);
}