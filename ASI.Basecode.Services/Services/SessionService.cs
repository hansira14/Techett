using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;

namespace ASI.Basecode.Services.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }
}