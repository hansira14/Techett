using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class FeedbackRepository : BaseRepository, IFeedbackRepository
{
    public FeedbackRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    
}