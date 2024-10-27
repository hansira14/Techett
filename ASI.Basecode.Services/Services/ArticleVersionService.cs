using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;

namespace ASI.Basecode.Services.Services;

public class ArticleVersionService : IArticleVersionService
{
    private readonly IArticleVersionRepository _articleVersionRepository;
    public ArticleVersionService(IArticleVersionRepository articleVersionRepository)
    {
        _articleVersionRepository = articleVersionRepository;
    }
}