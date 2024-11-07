using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data.Repositories;

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<Comment> GetAllComments()
    {
        return this.GetDbSet<Comment>().Include(c => c.User);
    }
    public void AddComment(Comment comment)
    {
        this.GetDbSet<Comment>().Add(comment);
        this.UnitOfWork.SaveChanges();
    }
    public Comment GetCommentById(int id)
    {
        return this.GetDbSet<Comment>().Find(id);
    }
    public void DeleteComment(Comment comment)
    {
        this.GetDbSet<Comment>().Remove(comment);
        this.UnitOfWork.SaveChanges();
    }
}