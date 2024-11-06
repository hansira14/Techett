using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;

namespace ASI.Basecode.Services.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    public AssignmentService(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }
}