using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;

namespace ASI.Basecode.Services.Services;

public class PreferenceService : IPreferenceService
{
    private readonly IPreferenceRepository _preferenceRepository;
    public PreferenceService(IPreferenceRepository preferenceRepository)
    {
        _preferenceRepository = preferenceRepository;
    }
}