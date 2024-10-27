using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class SessionRepository : BaseRepository, ISessionRepository
{
    public SessionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}