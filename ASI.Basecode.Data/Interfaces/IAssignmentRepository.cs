using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface IAssignmentRepository
{
    IQueryable<Assignment> GetAllAssignments();
    Assignment GetAssignmentById(int id);
    Assignment GetAssignmentByTicketId(int ticketId);
    void AddAssignment(Assignment assignment);
    void UpdateAssignment(Assignment assignment);
    void DeleteAssignment(Assignment assignment);
    IQueryable<Assignment> GetUserAssignments(int userId);
}