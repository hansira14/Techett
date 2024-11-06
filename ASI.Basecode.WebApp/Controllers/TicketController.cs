using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class TicketController : ControllerBase<TicketController>
{
    private readonly ITicketService _ticketService;

    public TicketController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        ITicketService ticketService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _ticketService = ticketService;
    }

    public IActionResult Index()
    {
        return View();
    }
}