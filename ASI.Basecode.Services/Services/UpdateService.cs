using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;

namespace ASI.Basecode.Services.Services;

public class UpdateService : IUpdateService
{
    private readonly IUpdateRepository _updateRepository;
    public UpdateService(IUpdateRepository updateRepository)
    {
        _updateRepository = updateRepository;
    }
}