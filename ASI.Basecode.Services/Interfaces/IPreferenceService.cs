namespace ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;

public interface IPreferenceService
{
    PreferenceViewModel GetPreferenceByUserId(int userId);
    void UpdatePreference(PreferenceViewModel preference, int userId);
}