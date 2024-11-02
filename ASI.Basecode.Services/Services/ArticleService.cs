using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace ASI.Basecode.Services.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleVersionRepository _articleVersionRepository;
    private readonly IMapper _mapper;

    public ArticleService(IArticleRepository articleRepository, 
                         IArticleVersionRepository articleVersionRepository,
                         IMapper mapper)
    {
        _articleRepository = articleRepository;
        _articleVersionRepository = articleVersionRepository;
        _mapper = mapper;
    }

    public IEnumerable<ArticleViewModel> GetAllArticles()
    {
        try
        {
            var articles = _articleRepository.GetAllArticles()
                                           .Include(a => a.CreatedByNavigation)
                                           .ToList();
            return _mapper.Map<IEnumerable<ArticleViewModel>>(articles);
        }
        catch (Exception ex)
        {
            // Add proper logging here
            throw;
        }
    }

    public ArticleDetailViewModel GetArticleById(int id)
    {
        var article = _articleRepository.GetArticleById(id);
        var versions = _articleVersionRepository.GetArticleVersions(id);
        
        return new ArticleDetailViewModel
        {
            Article = _mapper.Map<ArticleViewModel>(article),
            Versions = _mapper.Map<IEnumerable<ArticleVersionViewModel>>(versions)
        };
    }

    public void CreateArticle(ArticleViewModel model, int userId)
    {
        var article = _mapper.Map<Article>(model);
        article.CreatedBy = userId;
        article.CreatedOn = DateTime.Now;
        _articleRepository.AddArticle(article);
    }

    public void UpdateArticle(ArticleViewModel model, int userId)
    {
        var existingArticle = _articleRepository.GetArticleById(model.ArticleId);
        if (existingArticle != null)
        {
            // Create version before updating
            var version = new ArticleVersion
            {
                ArticleId = model.ArticleId,
                Title = existingArticle.Title,
                Content = existingArticle.Content,
                VersionedBy = userId
            };
            _articleVersionRepository.AddArticleVersion(version);

            // Update article
            var article = _mapper.Map<Article>(model);
            _articleRepository.UpdateArticle(article);
        }
    }

    public void DeleteArticle(int id)
    {
        var article = _articleRepository.GetArticleById(id);
        if (article != null)
        {
            _articleRepository.DeleteArticle(article);
        }
    }
}