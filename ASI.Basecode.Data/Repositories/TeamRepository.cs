using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class TeamRepository : BaseRepository, ITeamRepository
{
    public TeamRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}