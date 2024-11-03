using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class UpdateRepository : BaseRepository, IUpdateRepository
{
    public UpdateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<Update> GetAllUpdates()
    {
        return this.GetDbSet<Update>();
    }
    public void AddUpdate(Update update)
    {
        this.GetDbSet<Update>().Add(update);
        this.UnitOfWork.SaveChanges();
    }
    public Update GetUpdateById(int id)
    {
        return this.GetDbSet<Update>().Find(id);
    }
}