using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface ICommentRepository
{
    IQueryable<Comment> GetAllComments();
    void AddComment(Comment comment);
    Comment GetCommentById(int id);
    void DeleteComment(Comment comment);
}