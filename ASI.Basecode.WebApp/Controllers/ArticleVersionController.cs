using System;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class ArticleVersionController : ControllerBase<ArticleVersionController>
{
    private readonly IArticleVersionService _articleVersionService;
    public ArticleVersionController(IHttpContextAccessor httpContextAccessor,
                                    ILoggerFactory loggerFactory,
                                    IConfiguration configuration,
                                    IMapper mapper,
                                    IArticleVersionService articleVersionService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _articleVersionService = articleVersionService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetVersion(int versionId)
    {
        try
        {
            var version = _articleVersionService.GetVersionById(versionId);
            if (version == null || version.VersionedByNavigation == null)
            {
                return Json(new { success = false, message = "Version not found or user information is missing" });
            }

            return Json(new
            {
                versionId = version.VersionId,
                title = version.Title,
                content = version.Content,
                versionDate = version.VersionDate,
                versionedByName = version.VersionedByNavigation.Fname + " " + version.VersionedByNavigation.Lname
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting version details");
            return Json(new { success = false, message = "Failed to load version details" });
        }
    }
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost]
    public IActionResult RestoreVersion(int versionId)
    {
        try
        {
            var userId = GetCurrentUserId();
            _articleVersionService.RestoreVersion(versionId, userId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring version");
            return Json(new { success = false, message = "Failed to restore version" });
        }
    }
}