using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IUpdateService _updateService;

    public TicketService(ITicketRepository ticketRepository, IUserService userService, IMapper mapper, IUpdateService updateService)
    {
        _ticketRepository = ticketRepository;
        _userService = userService;
        _mapper = mapper;
        _updateService = updateService;
    }

    public IEnumerable<TicketViewModel> GetAllTickets()
    {
        var tickets = _ticketRepository.GetAllTickets();
        return _mapper.Map<IEnumerable<TicketViewModel>>(tickets);
    }

    public TicketViewModel GetTicketById(int id)
    {
        var ticket = _ticketRepository.GetTicketById(id);
        return _mapper.Map<TicketViewModel>(ticket);
    }

    public int CreateTicket(TicketViewModel model, int userId)
    {
        var ticket = _mapper.Map<Ticket>(model);
        ticket.CreatedBy = userId;
        ticket.CreatedOn = DateTime.Now;
        ticket.Status = "Open";
        ticket.IsActive = true;

        _ticketRepository.AddTicket(ticket);
        return ticket.TicketId;
    }

    public void UpdateTicket(TicketViewModel model, int userId)
    {
        var ticket = _ticketRepository.GetTicketById(model.TicketId);
        if (ticket == null)
            throw new InvalidOperationException("Ticket not found");

        var statusChanged = !string.Equals(ticket.Status, model.Status, StringComparison.OrdinalIgnoreCase);
        var priorityChanged = ticket.Priority != model.Priority;
        var categoryChanged = !string.Equals(ticket.Category, model.Category, StringComparison.OrdinalIgnoreCase);

        if (statusChanged || priorityChanged || categoryChanged)
        {
            var message = GenerateUpdateMessage(userId, ticket.TicketId, 
                statusChanged ? model.Status : null,
                priorityChanged ? model.Priority : null,
                categoryChanged ? model.Category : null);
            
            _updateService.AddUpdate(ticket.TicketId, model.Status, model.Priority, message, userId);
        }

        ticket.Title = model.Title;
        ticket.Content = model.Content;
        ticket.Category = model.Category;
        ticket.Priority = model.Priority;
        ticket.Status = model.Status;
        ticket.UpdatedOn = DateTime.Now;

        if (model.Status == "Resolved" && ticket.ResolvedOn == null)
        {
            ticket.ResolvedOn = DateTime.Now;
            ticket.ResolvedBy = userId;
        }

        _ticketRepository.UpdateTicket(ticket);
    }

    private string GenerateUpdateMessage(int userId, int ticketId, string newStatus, int? newPriority, string newCategory)
    {
        var user = _userService.GetUserById(userId);
        var userName = $"{user.Fname} {user.Lname}";
        
        var changes = new List<string>();
        
        if (newStatus != null)
            changes.Add($"status to {newStatus}");
        if (newPriority.HasValue)
            changes.Add($"priority to {GetPriorityText(newPriority.Value)}");
        if (newCategory != null)
            changes.Add($"category to {newCategory}");
        
        return $"{userName} updated Ticket #{ticketId} " + string.Join(" and ", changes);
    }

    private string GetPriorityText(int priority)
    {
        return priority switch
        {
            1 => "Low",
            2 => "Medium",
            3 => "High",
            _ => priority.ToString()
        };
    }

    public void DeleteTicket(int id)
    {
        var ticket = _ticketRepository.GetTicketById(id);
        if (ticket == null)
            throw new InvalidOperationException("Ticket not found");

        _ticketRepository.DeleteTicket(ticket);
    }

    public PaginatedTicketsViewModel GetPaginatedTickets(string searchTerm, int page, int pageSize)
    {
        var query = _ticketRepository.GetAllTickets().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(t => 
                t.Title.ToLower().Contains(searchTerm) ||
                t.Content.ToLower().Contains(searchTerm) ||
                t.Category.ToLower().Contains(searchTerm) ||
                t.Status.ToLower().Contains(searchTerm)
            );
        }

        var totalTickets = query.Count();

        var tickets = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedTicketsViewModel
        {
            Tickets = _mapper.Map<IEnumerable<TicketViewModel>>(tickets),
            TotalTickets = totalTickets,
            CurrentPage = page,
            PageSize = pageSize,
            SearchTerm = searchTerm
        };
    }
}