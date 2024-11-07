using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class AssignmentRepository : BaseRepository, IAssignmentRepository
{
    public AssignmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public IQueryable<Assignment> GetAllAssignments()
    {
        return this.GetDbSet<Assignment>();
    }

    public Assignment GetAssignmentById(int id)
    {
        return this.GetDbSet<Assignment>().Find(id);
    }

    public Assignment GetAssignmentByTicketId(int ticketId)
    {
        return this.GetDbSet<Assignment>().FirstOrDefault(a => a.TicketId == ticketId);
    }

    public void AddAssignment(Assignment assignment)
    {
        this.GetDbSet<Assignment>().Add(assignment);
        this.UnitOfWork.SaveChanges();
    }

    public void UpdateAssignment(Assignment assignment)
    {
        var existing = this.GetDbSet<Assignment>().Find(assignment.AssignmentId);
        if (existing != null)
        {
            existing.AssignedTo = assignment.AssignedTo;
            existing.AssignedBy = assignment.AssignedBy;
            existing.AssignedOn = assignment.AssignedOn;
            this.UnitOfWork.SaveChanges();
        }
    }

    public void DeleteAssignment(Assignment assignment)
    {
        this.GetDbSet<Assignment>().Remove(assignment);
        this.UnitOfWork.SaveChanges();
    }
}