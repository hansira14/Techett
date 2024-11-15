using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces;

public interface IArticleService
{
    IEnumerable<ArticleViewModel> GetAllArticles();
    ArticleViewModel GetArticleById(int id);
    int CreateArticle(ArticleViewModel model, int userId);
    void UpdateArticle(ArticleViewModel model, int userId);
    void DeleteArticle(int id);
}
