using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class TeamRepository : BaseRepository, ITeamRepository
{
    public TeamRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<Team> GetAllTeams()
    {
        return this.GetDbSet<Team>();
    }
    public Team GetTeamById(int id)
    {
        return this.GetDbSet<Team>().Find(id);
    }
    public void DeleteTeam(Team team)
    {
        this.GetDbSet<Team>().Remove(team);
        this.UnitOfWork.SaveChanges();
    }
    public void AddTeam(Team team)
    {
        this.GetDbSet<Team>().Add(team);
        this.UnitOfWork.SaveChanges();
    }
}