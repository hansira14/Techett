using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories;

public class TicketRepository : BaseRepository, ITicketRepository
{
    public TicketRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<Ticket> GetTickets() => this.GetDbSet<Ticket>().AsQueryable();

    public async Task<bool> TicketExists(string title) => await this.GetDbSet<Ticket>().AnyAsync(x => x.Title == title);

    public async Task AddTicket(Ticket ticket)
    {
        await this.GetDbSet<Ticket>().AddAsync(ticket);
        UnitOfWork.SaveChanges();
    }

    public void UpdateTicket(Ticket ticket)
    {
        var existingTicket = this.GetDbSet<Ticket>()
            .Where(x => x.TicketId == ticket.TicketId)
            .FirstOrDefault();

        existingTicket = ticket;
        this.GetDbSet<Ticket>().Update(existingTicket);

        UnitOfWork.SaveChanges();

    }
    public void DeleteTicket(long id, int t_id)
    {
        var existingTicket = this.GetDbSet<Ticket>().Where(x => x.TicketId == id).FirstOrDefault()
              ?? throw new KeyNotFoundException($"The artitcle with the id '{id}' is not found!");

        this.GetDbSet<Ticket>().Remove(existingTicket);
        UnitOfWork.SaveChanges();

    }

}