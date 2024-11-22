using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ITicketService
    {
        IEnumerable<TicketViewModel> GetAllTickets();
        TicketViewModel GetTicketById(int id);
        void UpdateTicket(TicketViewModel ticket, int userId);
        void DeleteTicket(int id);
        int CreateTicket(TicketViewModel model, int userId);
        PaginatedTicketsViewModel GetPaginatedTickets(string searchTerm, int page, int pageSize);
    }
}