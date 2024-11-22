using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ASI.Basecode.Services.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IUpdateService _updateService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TicketService(ITicketRepository ticketRepository, IUserService userService, IMapper mapper, IUpdateService updateService, IHttpContextAccessor httpContextAccessor)
    {
        _ticketRepository = ticketRepository;
        _userService = userService;
        _mapper = mapper;
        _updateService = updateService;
        _httpContextAccessor = httpContextAccessor;
    }

    public IEnumerable<TicketViewModel> GetAllTickets()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userRole = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        
        if (string.IsNullOrEmpty(userRole) || !int.TryParse(userIdClaim, out int userId))
        {
            return Enumerable.Empty<TicketViewModel>();
        }

        var tickets = _ticketRepository.GetAllTickets(userRole, userId);
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

    public PaginatedTicketsViewModel GetPaginatedTickets(string searchTerm, int page, int pageSize, 
        string[] categories = null, string[] priorities = null,
        string sortColumn = null, string sortDirection = "asc")
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userRole = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        
        if (string.IsNullOrEmpty(userRole) || !int.TryParse(userIdClaim, out int userId))
        {
            return new PaginatedTicketsViewModel
            {
                Tickets = Enumerable.Empty<TicketViewModel>(),
                TotalTickets = 0,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        var query = _ticketRepository.GetAllTickets(userRole, userId).AsQueryable();

        // Apply search filter
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

        // Apply category filter
        if (categories != null && categories.Length > 0)
        {
            query = query.Where(t => categories.Contains(t.Category));
        }

        // Apply priority filter
        if (priorities != null && priorities.Length > 0)
        {
            var priorityNumbers = priorities.Select(int.Parse).ToArray();
            query = query.Where(t => priorityNumbers.Contains(t.Priority));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(sortColumn))
        {
            query = sortColumn.ToLower() switch
            {
                "title" => sortDirection == "asc" 
                    ? query.OrderBy(t => t.Title) 
                    : query.OrderByDescending(t => t.Title),
                
                "content" => sortDirection == "asc" 
                    ? query.OrderBy(t => t.Content) 
                    : query.OrderByDescending(t => t.Content),
                
                "status" => sortDirection == "asc" 
                    ? query.OrderBy(t => t.Status) 
                    : query.OrderByDescending(t => t.Status),
                
                "category" => sortDirection == "asc" 
                    ? query.OrderBy(t => t.Category) 
                    : query.OrderByDescending(t => t.Category),
                
                "priority" => sortDirection == "asc" 
                    ? query.OrderBy(t => t.Priority) 
                    : query.OrderByDescending(t => t.Priority),

                 "assignedto" => sortDirection == "asc"
                    ? query.OrderBy(t => t.Assignments
                        .Select(a => a.AssignedToNavigation.Fname + " " + a.AssignedToNavigation.Lname)
                        .FirstOrDefault() ?? "zzz")
                    : query.OrderByDescending(t => t.Assignments
                        .Select(a => a.AssignedToNavigation.Fname + " " + a.AssignedToNavigation.Lname)
                        .FirstOrDefault() ?? ""),
                
                
                _ => query.OrderByDescending(t => t.CreatedOn) // Default sort
            };
        }
        else
        {
            // Default sorting by creation date if no sort column specified
            query = query.OrderByDescending(t => t.CreatedOn);
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
            SearchTerm = searchTerm,
            SelectedCategories = categories,
            SelectedPriorities = priorities,
            SortColumn = sortColumn,
            SortDirection = sortDirection
        };
    }
}