using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces;

public interface ICommentService
{
    IEnumerable<CommentViewModel> GetTicketComments(int ticketId);
    void AddComment(CommentViewModel comment);
    void DeleteComment(int commentId);
}