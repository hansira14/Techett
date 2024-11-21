using System;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
    
namespace ASI.Basecode.Services.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IUpdateService _updateService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        IUpdateService updateService,
        IUserService userService,
        IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _updateService = updateService;
        _userService = userService;
        _mapper = mapper;
    }

    public AssignmentViewModel GetAssignmentByTicketId(int ticketId)
    {
        var assignment = _assignmentRepository.GetAssignmentByTicketId(ticketId);
        return _mapper.Map<AssignmentViewModel>(assignment);
    }

    public void AssignTicket(AssignmentViewModel model, int currentUserId)
    {
        var assignment = _mapper.Map<Assignment>(model);
        assignment.AssignedBy = currentUserId;
        assignment.AssignedOn = DateTime.Now;
        _assignmentRepository.AddAssignment(assignment);

        // Generate update record
        var assignedUser = _userService.GetUserById(model.AssignedTo);
        var message = $"{_userService.GetUserById(currentUserId).Fname} {_userService.GetUserById(currentUserId).Lname} " +
                     $"assigned the ticket to {assignedUser.Fname} {assignedUser.Lname}";
        
        _updateService.AddUpdate(model.TicketId, null, null, message, currentUserId);
    }

    public void UpdateAssignment(AssignmentViewModel model, int currentUserId)
    {
        var assignment = _mapper.Map<Assignment>(model);
        assignment.AssignedBy = currentUserId;
        assignment.AssignedOn = DateTime.Now;
        _assignmentRepository.UpdateAssignment(assignment);
    }

    public void RemoveAssignment(int ticketId, int currentUserId)
    {
        var assignment = _assignmentRepository.GetAssignmentByTicketId(ticketId);
        if (assignment != null)
        {
            var assignedUser = _userService.GetUserById(assignment.AssignedTo);
            _assignmentRepository.DeleteAssignment(assignment);

            // Generate update record
            var message = $"{_userService.GetUserById(currentUserId).Fname} {_userService.GetUserById(currentUserId).Lname} " +
                         $"removed the ticket assignment from {assignedUser.Fname} {assignedUser.Lname}";
            
            _updateService.AddUpdate(ticketId, null, null, message, currentUserId);
        }
    }
}