using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class UpdateController : ControllerBase<UpdateController>
{
    private readonly IUpdateService _updateService;

    public UpdateController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        IUpdateService updateService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _updateService = updateService;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetTicketUpdates(int ticketId)
    {
        var updates = _updateService.GetTicketUpdates(ticketId);
        return Json(updates);
    }
}