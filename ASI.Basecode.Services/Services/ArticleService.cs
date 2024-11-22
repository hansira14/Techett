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
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public ArticleService(IArticleRepository articleRepository, 
                         IMapper mapper,
                         IUserRepository userRepository)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
        _userRepository = userRepository;
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
            throw;
        }
    }

    public ArticleViewModel GetArticleById(int id)
    {
        var article = _articleRepository.GetArticleById(id);
        return _mapper.Map<ArticleViewModel>(article);
    }

    public int CreateArticle(ArticleViewModel model, int userId)
    {
        var article = _mapper.Map<Article>(model);
        article.CreatedBy = userId;
        article.CreatedOn = DateTime.Now;

        var user = _userRepository.GetById(model.CreatedBy);
        
        model.CreatedByProfilePicUrl = string.IsNullOrEmpty(user.ProfilePicUrl)
            ? AvatarHelper.GetInitialAvatar(user.Fname, user.Lname)
            : user.ProfilePicUrl;

        _articleRepository.AddArticle(article);
        return article.ArticleId;
    }

    public void UpdateArticle(ArticleViewModel model, int userId)
    {
        var article = _mapper.Map<Article>(model);
        _articleRepository.UpdateArticle(article);
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