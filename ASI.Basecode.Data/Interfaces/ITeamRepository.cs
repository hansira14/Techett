using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface ITeamRepository
{
    IQueryable<Team> GetAllTeams();
    void AddTeam(Team team);
    Team GetTeamById(int id);
    void DeleteTeam(Team team);
}