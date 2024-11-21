using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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

    [HttpGet]
    public IActionResult GetPreference()
    {
        // Assuming the user's ID is retrieved from the authenticated context
        var userId = GetAuthenticatedUserId();

        if (userId == null)
            return Unauthorized("User is not authenticated.");

        var preference = _preferenceService.GetPreferenceByUserId(userId.Value);

        if (preference == null)
            return NotFound($"Preference for user ID {userId.Value} not found.");

        return Ok(preference);
    }

    [HttpPut]
    public IActionResult UpdatePreference([FromBody] PreferenceViewModel preferenceViewModel)
    {
        var userId = GetAuthenticatedUserId();

        if (userId == null)
            return Unauthorized("User is not authenticated.");

        if (preferenceViewModel == null)
            return BadRequest("Preference data is missing.");

        _preferenceService.UpdatePreference(preferenceViewModel, userId.Value);

        return Ok("Preference updated successfully.");
    }

    private int? GetAuthenticatedUserId()
    {
        var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == "UserId");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return null;
        }

        return userId;
    }
}