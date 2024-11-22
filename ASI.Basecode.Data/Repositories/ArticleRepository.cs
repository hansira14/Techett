using System.Linq;
using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;
using ASI.Basecode.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data.Repositories;

public class ArticleRepository : BaseRepository, IArticleRepository
{
    public ArticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<Article> GetAllArticles()
    {
        return this.GetDbSet<Article>()
            .Where(a => !a.IsDeleted.HasValue || !a.IsDeleted.Value);
    }
    public void AddArticle(Article article){
        this.GetDbSet<Article>().Add(article);
        this.UnitOfWork.SaveChanges();
    }
    public void UpdateArticle(Article article)
    {
        var existingArticle = this.GetDbSet<Article>().Find(article.ArticleId);
        if (existingArticle != null)
        {
            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            this.UnitOfWork.SaveChanges();
        }
    }
    public Article GetArticleById(int id)
    {
        return this.GetDbSet<Article>()
            .Include(a => a.CreatedByNavigation)
            .Where(a => (!a.IsDeleted.HasValue || !a.IsDeleted.Value) && a.ArticleId == id)
            .FirstOrDefault();
    }
}