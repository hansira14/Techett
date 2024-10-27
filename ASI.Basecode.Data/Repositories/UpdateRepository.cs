using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class UpdateRepository : BaseRepository, IUpdateRepository
{
    public UpdateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
