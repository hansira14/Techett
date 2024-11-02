using ASI.Basecode.Data.Models;
using System.Linq;
namespace ASI.Basecode.Data.Interfaces;

public interface IArticleRepository
{
    IQueryable<Article> GetAllArticles();
    void AddArticle(Article article);
    void UpdateArticle(Article article);
    Article GetArticleById(int id);
    void DeleteArticle(Article article);
}
