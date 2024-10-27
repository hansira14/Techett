using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Repositories;

public class ArticleRepository : BaseRepository, IArticleRepository
{
    public ArticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}