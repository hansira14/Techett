using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces;

public interface ITicketRepository
{
    IQueryable<Ticket> GetAllTickets();
    void AddTicket(Ticket ticket);
    Ticket GetTicketById(int id);
    void UpdateTicket(Ticket ticket);
    void DeleteTicket(Ticket ticket);
    int GetResolvedTicketsCount(int userId);
}