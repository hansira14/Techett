using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class FeedbackController : ControllerBase<FeedbackController>
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        IFeedbackService feedbackService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _feedbackService = feedbackService;
    }
    public IActionResult Index()
    {
        return View();
    }
}