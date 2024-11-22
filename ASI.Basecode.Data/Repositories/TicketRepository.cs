using System;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data.Repositories;

public class TicketRepository : BaseRepository, ITicketRepository
{
    public TicketRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public IQueryable<Ticket> GetAllTickets(string userRole, int userId)
    {
        var query = this.GetDbSet<Ticket>()
            .Include(t => t.Assignments)
                .ThenInclude(a => a.AssignedToNavigation)
            .Include(t => t.CreatedByNavigation)
            .Include(t => t.ResolvedByNavigation);

        switch (userRole)
        {
            case "User":
                return query.Where(t => t.CreatedBy == userId);
            case "Agent":
                return query.Where(t => t.Assignments.Any(a => a.AssignedTo == userId));
            case "Admin":
            case "SuperAdmin":
                return query;
            default:
                return Enumerable.Empty<Ticket>().AsQueryable();
        }
    }
    public void AddTicket(Ticket ticket)
    {
        this.GetDbSet<Ticket>().Add(ticket);
        this.UnitOfWork.SaveChanges();
    }
    public void UpdateTicket(Ticket ticket)
    {
        var existingTicket = this.GetDbSet<Ticket>().Find(ticket.TicketId);
        if (existingTicket != null)
        {
            this.Context.Entry(existingTicket).CurrentValues.SetValues(ticket);
            this.UnitOfWork.SaveChanges();
        }
    }
    public Ticket GetTicketById(int id)
    {
        return this.GetDbSet<Ticket>()
            .Include(t => t.CreatedByNavigation)
            .Include(t => t.ResolvedByNavigation)
            .Include(t => t.Assignments)
                .ThenInclude(a => a.AssignedToNavigation)
            .FirstOrDefault(t => t.TicketId == id);
    }
    public void DeleteTicket(Ticket ticket)
    {
        this.GetDbSet<Ticket>().Remove(ticket);
        this.UnitOfWork.SaveChanges();
    }
    public int GetResolvedTicketsCount(int userId)
    {
        return this.GetDbSet<Ticket>()
            .Count(t => t.ResolvedBy == userId && 
                       t.Status == "Resolved");
    }
}