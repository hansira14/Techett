using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class ArticleVersionRepository : BaseRepository, IArticleVersionRepository
{
    public ArticleVersionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}