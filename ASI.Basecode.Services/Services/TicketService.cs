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

    public TicketService(ITicketRepository ticketRepository, IUserService userService, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _userService = userService;
        _mapper = mapper;
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

    public void DeleteTicket(int id)
    {
        var ticket = _ticketRepository.GetTicketById(id);
        if (ticket == null)
            throw new InvalidOperationException("Ticket not found");

        _ticketRepository.DeleteTicket(ticket);
    }
}