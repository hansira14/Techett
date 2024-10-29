using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;

namespace ASI.Basecode.Services.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    public TeamService(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }
}