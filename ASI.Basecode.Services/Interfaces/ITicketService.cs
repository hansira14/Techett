using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ITicketService
    {
        IEnumerable<TicketViewModel> GetAllTickets();
        TicketViewModel GetTicketById(int id);
        void CreateTicket(TicketViewModel ticket, int userId);
        void UpdateTicket(TicketViewModel ticket, int userId);
        void DeleteTicket(int id);
    }
}