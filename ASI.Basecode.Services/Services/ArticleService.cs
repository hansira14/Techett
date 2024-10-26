using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
namespace ASI.Basecode.Services.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    public ArticleService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }
}