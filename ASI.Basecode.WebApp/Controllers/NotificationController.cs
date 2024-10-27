using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class NotificationController : ControllerBase<NotificationController>
{
    private readonly INotificationService _notificationService;

    public NotificationController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        INotificationService notificationService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _notificationService = notificationService;
    }
    public IActionResult Index()
    {
        return View();
    }
}