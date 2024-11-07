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
    private readonly IMapper _mapper;

    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
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
    }

    public void UpdateAssignment(AssignmentViewModel model, int currentUserId)
    {
        var assignment = _mapper.Map<Assignment>(model);
        assignment.AssignedBy = currentUserId;
        assignment.AssignedOn = DateTime.Now;
        _assignmentRepository.UpdateAssignment(assignment);
    }
}