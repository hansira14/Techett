using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;

namespace ASI.Basecode.Services.Services;

public class PreferenceService : IPreferenceService
{
    private readonly IPreferenceRepository _preferenceRepository;
    private readonly IMapper _mapper;

    public PreferenceService(IPreferenceRepository preferenceRepository, IMapper mapper)
    {
        _preferenceRepository = preferenceRepository;
        _mapper = mapper;
    }

    public PreferenceViewModel GetPreferenceByUserId(int userId)
    {
        var preference = _preferenceRepository.GetPreferenceByUserId(userId);
        return _mapper.Map<PreferenceViewModel>(preference);
    }

    public void UpdatePreference(PreferenceViewModel preferenceViewModel, int userId)
    {
        var preference = _preferenceRepository.GetPreferenceByUserId(userId);

        if (preference != null)
        {
            _mapper.Map(preferenceViewModel, preference);
            _preferenceRepository.UpdatePreference(preference);
        }
    }

}