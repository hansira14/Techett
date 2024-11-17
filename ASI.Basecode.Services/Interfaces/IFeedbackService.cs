namespace ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;

public interface IFeedbackService
{
    void AddFeedback(FeedbackViewModel feedback);
    FeedbackViewModel GetFeedbackByTicketId(int ticketId);
    bool HasUserSubmittedFeedback(int ticketId, int userId);
}