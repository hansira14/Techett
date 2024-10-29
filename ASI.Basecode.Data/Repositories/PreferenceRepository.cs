using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class PreferenceRepository : BaseRepository, IPreferenceRepository
{
    public PreferenceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}