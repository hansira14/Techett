using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class FeedbackRepository : BaseRepository, IFeedbackRepository
{
    public FeedbackRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<Feedback> GetAllFeedbacks()
    {
        return this.GetDbSet<Feedback>();
    }
    public void AddFeedback(Feedback feedback)
    {
        this.GetDbSet<Feedback>().Add(feedback);
        this.UnitOfWork.SaveChanges();
    }
    public Feedback GetFeedbackById(int id)
    {
        return this.GetDbSet<Feedback>().Find(id);
    }
    public void DeleteFeedback(Feedback feedback)
    {
        this.GetDbSet<Feedback>().Remove(feedback);
        this.UnitOfWork.SaveChanges();
    }   
}