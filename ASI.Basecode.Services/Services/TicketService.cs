using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using AutoMapper;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.Services;

public class TicketService : ITicketService
{
    
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    /// <summary>
    /// Retrieves the ticket.
    /// </summary>
    /// <param name="thruCreation">if set to <c>true</c> [thru creation].</param>
    /// <returns></returns>
    public IEnumerable<TicketViewModel> RetrieveTicket(bool thruCreation)
    {
        var data = _ticketRepository.GetTickets().Select(t => _mapper.Map(t, new TicketViewModel()));
        return data;

    }

    /// <summary>
    /// Adds the ticket.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="userId">The user identifier.</param>
    public async Task AddTicket(TicketViewModel model, string userId)
    {
        

    }

    /// <summary>
    /// Update2s the specified model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="userId">The user identifier.</param>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">The ticket '{model.Title}' is not found!</exception>
    public async Task Update2(TicketViewModel model, string userId)
    {
        
    }

    /// <summary>
    /// Assigns the agent.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="userId">The user identifier.</param>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">The ticket '{model.Title}' is not found!</exception>
    public void AssignAgent(long ticketId, int agentId)
    {        
        var existingTicket = _ticketRepository.GetTickets().FirstOrDefault(t => t.TicketId == ticketId);

        if (existingTicket == null)
        {
            throw new KeyNotFoundException($"The ticket with ID '{ticketId}' is not found!");
        }

        _ticketRepository.UpdateTicket(existingTicket);
    }

    /// <summary>
    /// Updates the status and priority level of a ticket assigned to an agent.
    /// </summary>
    /// <param name="ticketId">The ticket identifier.</param>
    /// <param name="status">The new status.</param>
    /// <param name="priorityLevel">The new priority level.</param>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">The ticket is not found!</exception>
    public void UpdateStatusPriority(long ticketId, string status, string priorityLevel)
    {
    }


    

    /// <summary>
    /// Deletes the specified model.
    /// </summary>
    /// <param name="model">The model.</param>
    public async Task Delete(TicketViewModel model)
    {
    }
}