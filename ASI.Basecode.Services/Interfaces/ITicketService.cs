using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces;

public interface ITicketService
{
    IEnumerable<TicketViewModel> RetrieveTicket(bool thruCreation);
    Task AddTicket(TicketViewModel model, string userId);
    Task Update2(TicketViewModel model, string userId);
    Task Delete(TicketViewModel model);
    void AssignAgent(long ticketId, int agentId);
    void UpdateStatusPriority(long ticketId, string status, string priorityLevel);
    
}