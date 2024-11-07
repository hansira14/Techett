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
    public IQueryable<Ticket> GetAllTickets()
    {
        return this.GetDbSet<Ticket>()
            .Include(t => t.Assignments)
                .ThenInclude(a => a.AssignedToNavigation)
            .Include(t => t.CreatedByNavigation)
            .Include(t => t.ResolvedByNavigation);
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
            existingTicket.Title = ticket.Title;
            existingTicket.Content = ticket.Content;
            existingTicket.Status = ticket.Status;
            existingTicket.Category = ticket.Category;
            existingTicket.Priority = ticket.Priority;
            existingTicket.UpdatedOn = DateTime.Now;
            existingTicket.ResolvedOn = ticket.ResolvedOn;
            existingTicket.ResolvedBy = ticket.ResolvedBy;

            this.UnitOfWork.SaveChanges();
        }
    }
    public Ticket GetTicketById(int id)
    {
        return this.GetDbSet<Ticket>().Find(id);
    }
    public void DeleteTicket(Ticket ticket)
    {
        this.GetDbSet<Ticket>().Remove(ticket);
        this.UnitOfWork.SaveChanges();
    }
}