using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface IUpdateRepository
{
    IQueryable<Update> GetAllUpdates();
    void AddUpdate(Update update);
    Update GetUpdateById(int id);
}