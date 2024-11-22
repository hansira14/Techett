using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface IFeedbackRepository
{
    IQueryable<Feedback> GetAllFeedbacks();
    void AddFeedback(Feedback feedback);
    Feedback GetFeedbackById(int id);
    IQueryable<Feedback> GetUserFeedbacks(int userId);
}