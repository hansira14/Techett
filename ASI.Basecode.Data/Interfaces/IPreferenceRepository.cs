using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface IPreferenceRepository
{
    void AddPreference(Preference preference);
    void UpdatePreference(Preference preference);
    Preference GetPreferenceByUserId(int userId);
}