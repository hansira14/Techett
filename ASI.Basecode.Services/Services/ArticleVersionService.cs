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
    private readonly IMapper _mapper;

    public ArticleVersionService(IArticleVersionRepository articleVersionRepository,
                               IMapper mapper)
    {
        _articleVersionRepository = articleVersionRepository;
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
    
}
