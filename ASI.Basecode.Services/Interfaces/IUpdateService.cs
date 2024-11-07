using System.Collections.Generic;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.Interfaces;

public interface IUpdateService
{
    IEnumerable<UpdateViewModel> GetTicketUpdates(int ticketId);
    void AddUpdate(int ticketId, string status, int? priority, string message, int userId);
}