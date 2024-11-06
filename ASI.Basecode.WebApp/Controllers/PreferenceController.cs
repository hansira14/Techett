using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class PreferenceController : ControllerBase<PreferenceController>
{
    private readonly IPreferenceService _preferenceService;
    public PreferenceController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        IPreferenceService preferenceService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _preferenceService = preferenceService;
    }
    public IActionResult Index()
    {
        return View();
    }
}