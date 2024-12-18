using System;
using System.Linq;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ASI.Basecode.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ASI.Basecode.WebApp.Controllers;

public class AssignmentController : ControllerBase<AssignmentController>
{
    private readonly IAssignmentService _assignmentService;
    private readonly IUserService _userService;
    private readonly IAssignmentRepository _assignmentRepository;

    public AssignmentController(IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper,
                            IAssignmentService assignmentService,
                            IUserService userService,
                            IAssignmentRepository assignmentRepository) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _assignmentService = assignmentService;
        _userService = userService;
        _assignmentRepository = assignmentRepository;
    }
    [HttpGet]
    [Authorize(Policy = "RequireAdminRole")]
    public IActionResult GetAgents()
    {
        var agents = _userService.GetAllUsers().Where(u => u.Role.Equals("Agent", StringComparison.OrdinalIgnoreCase));
        return Json(agents);
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdminRole")]
    public IActionResult AssignTicket(AssignmentViewModel model)
    {
        try
        {
            var userId = GetCurrentUserId();
            _assignmentService.AssignTicket(model, userId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdminRole")]
    public IActionResult UpdateAssignment(AssignmentViewModel model)
    {
        try
        {
            var userId = GetCurrentUserId();
            _assignmentService.UpdateAssignment(model, userId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdminRole")]
    public IActionResult RemoveAssignment(int ticketId)
    {
        try
        {
            var userId = GetCurrentUserId();
            _assignmentService.RemoveAssignment(ticketId, userId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}