using System;
using System.Collections.Generic;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Services.Services;

public class ArticleVersionService : IArticleVersionService
{
    private readonly IArticleVersionRepository _articleVersionRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public ArticleVersionService(IArticleVersionRepository articleVersionRepository,
                               IArticleRepository articleRepository,
                               IMapper mapper)
    {
        _articleVersionRepository = articleVersionRepository;
        _articleRepository = articleRepository;
        _mapper = mapper;
    }

    public IEnumerable<ArticleVersionViewModel> GetArticleVersions(int articleId)
    {
        var versions = _articleVersionRepository.GetArticleVersions(articleId)
            .Include(v => v.VersionedByNavigation);
        return _mapper.Map<IEnumerable<ArticleVersionViewModel>>(versions);
    }

    public void CreateArticleVersion(int articleId, string title, string content, int userId)
    {
        var version = new ArticleVersion
        {
            ArticleId = articleId,
            Title = title,
            Content = content,
            VersionedBy = userId
        };
        _articleVersionRepository.AddArticleVersion(version);
    }

    public ArticleVersion GetVersionById(int versionId) 
    {
        return _articleVersionRepository.GetArticleVersionById(versionId);
    }
    
    public void RestoreVersion(int versionId, int userId)
    {
        var version = GetVersionById(versionId);
        if (version == null)
        {
            throw new Exception("Version not found");
        }

        // Create a new version with the current content before restoring
        var currentArticle = version.Article;
        CreateArticleVersion(
            currentArticle.ArticleId,
            currentArticle.Title,
            currentArticle.Content,
            userId
        );

        // Update the article with the version's content
        currentArticle.Title = version.Title;
        currentArticle.Content = version.Content;
        _articleVersionRepository.UpdateArticle(currentArticle);
    }
}
