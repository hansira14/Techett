using ASI.Basecode.Data.Models;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces;

public interface ITicketRepository
{
    
    public IQueryable<Ticket> GetTickets();
    Task<bool> TicketExists(string title);
    Task AddTicket(Ticket ticket);
    void UpdateTicket(Ticket ticket);
    void DeleteTicket(long id, int t_id);
}