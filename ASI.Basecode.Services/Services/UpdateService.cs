using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Services.Services;

public class UpdateService : IUpdateService
{
    private readonly IUpdateRepository _updateRepository;
    private readonly IMapper _mapper;

    public UpdateService(IUpdateRepository updateRepository, IMapper mapper)
    {
        _updateRepository = updateRepository;
        _mapper = mapper;
    }

    public IEnumerable<UpdateViewModel> GetTicketUpdates(int ticketId)
    {
        var updates = _updateRepository.GetAllUpdates()
            .Where(u => u.TicketId == ticketId)
            .Include(u => u.UpdatedByNavigation)
            .OrderByDescending(u => u.UpdatedOn)
            .Select(u => new UpdateViewModel
            {
                UpdateId = u.UpdateId,
                TicketId = u.TicketId,
                Status = u.Status,
                Priority = u.Priority,
                Message = u.Message,
                UpdatedOn = u.UpdatedOn,
                UpdatedBy = u.UpdatedBy,
                UpdatedByName = $"{u.UpdatedByNavigation.Fname} {u.UpdatedByNavigation.Lname}"
            });

        return updates;
    }

    public void AddUpdate(int ticketId, string status, int? priority, string message, int userId)
    {
        var update = new Update
        {
            TicketId = ticketId,
            Status = status,
            Priority = priority,
            Message = message,
            UpdatedOn = DateTime.Now,
            UpdatedBy = userId
        };

        _updateRepository.AddUpdate(update);
    }
}