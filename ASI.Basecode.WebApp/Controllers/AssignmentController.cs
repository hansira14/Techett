using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class AssignmentController : ControllerBase<AssignmentController>
{
    private readonly IAssignmentService _assignmentService;
    public AssignmentController(IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper,
                                IAssignmentService assignmentService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _assignmentService = assignmentService;
    }

    public IActionResult Index()
    {
        return View();
    }
}