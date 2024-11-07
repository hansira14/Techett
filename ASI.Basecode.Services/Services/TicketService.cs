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

    public void CreateTicket(TicketViewModel model, int userId)
    {
        var ticket = _mapper.Map<Ticket>(model);
        ticket.CreatedBy = userId;
        ticket.CreatedOn = DateTime.Now;
        ticket.Status = "Open";
        ticket.IsActive = true;

        _ticketRepository.AddTicket(ticket);
    }

    public void UpdateTicket(TicketViewModel model, int userId)
    {
        var ticket = _ticketRepository.GetTicketById(model.TicketId);
        if (ticket == null)
            throw new InvalidOperationException("Ticket not found");

        var statusChanged = ticket.Status != model.Status;
        var priorityChanged = ticket.Priority != model.Priority;

        if (statusChanged || priorityChanged)
        {
            var message = GenerateUpdateMessage(userId, ticket.TicketId, 
                statusChanged ? model.Status : null,
                priorityChanged ? model.Priority : null);
            
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

    private string GenerateUpdateMessage(int userId, int ticketId, string newStatus, int? newPriority)
    {
        var user = _userService.GetUserById(userId);
        var userName = $"{user.Fname} {user.Lname}";
        
        if (newStatus != null && newPriority.HasValue)
            return $"{userName} updated Ticket #{ticketId} status to {newStatus} and priority to {GetPriorityText(newPriority.Value)}";
        else if (newStatus != null)
            return $"{userName} updated Ticket #{ticketId} status to {newStatus}";
        else
            return $"{userName} updated Ticket #{ticketId} priority to {GetPriorityText(newPriority.Value)}";
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
}