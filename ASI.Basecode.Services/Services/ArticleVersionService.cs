using System.Collections.Generic;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;

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
        var versions = _articleVersionRepository.GetArticleVersions(articleId);
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
}