using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class TeamController : ControllerBase<TeamController>
{
    private readonly ITeamService _teamService;
    public TeamController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        ITeamService teamService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _teamService = teamService;
    }

    public IActionResult Index()
    {
        return View();
    }
}