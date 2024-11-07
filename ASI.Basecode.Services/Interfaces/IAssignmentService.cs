using System.Collections.Generic;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.Interfaces;

public interface IAssignmentService
{
    AssignmentViewModel GetAssignmentByTicketId(int ticketId);
    void AssignTicket(AssignmentViewModel model, int currentUserId);
    void UpdateAssignment(AssignmentViewModel model, int currentUserId);
    void RemoveAssignment(int ticketId, int currentUserId);
}