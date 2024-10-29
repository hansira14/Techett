using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}