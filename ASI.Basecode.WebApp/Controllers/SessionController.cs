using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class SessionController : ControllerBase<SessionController>
{
    private readonly ISessionService _sessionService;
    public SessionController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        ISessionService sessionService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _sessionService = sessionService;
    }

    public IActionResult Index()
    {
        return View();
    }
}