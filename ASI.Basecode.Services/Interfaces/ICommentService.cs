using System.Collections.Generic;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Services.Interfaces;

public interface ICommentService
{
    IEnumerable<CommentViewModel> GetTicketComments(int ticketId);
    void AddComment(CommentViewModel comment);
    void DeleteComment(int commentId);
    Comment GetCommentById(int commentId);
}