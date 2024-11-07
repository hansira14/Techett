using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class PreferenceRepository : BaseRepository, IPreferenceRepository
{
    public PreferenceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public void AddPreference(Preference preference)
    {
        this.GetDbSet<Preference>().Add(preference);
        this.UnitOfWork.SaveChanges();
    }
    public void UpdatePreference(Preference preference)
    {
        var existingPreference = this.GetDbSet<Preference>().Find(preference.PreferenceId);
        if (existingPreference != null)
        {
            existingPreference.IsEmailingOn = preference.IsEmailingOn;
            existingPreference.ViewType = preference.ViewType;
            this.UnitOfWork.SaveChanges();
        }
    }
    public Preference GetPreferenceByUserId(int userId)
    {
        return this.GetDbSet<Preference>().FirstOrDefault(p => p.UserId == userId);
    }
}