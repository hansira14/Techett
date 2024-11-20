using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class ArticleController : ControllerBase<ArticleController>
{
    private readonly IArticleService _articleService;
    private readonly IArticleVersionService _articleVersionService;
    private readonly IArticleAttachmentService _articleAttachmentService;

    public ArticleController(IHttpContextAccessor httpContextAccessor,
                           ILoggerFactory loggerFactory,
                           IConfiguration configuration,
                           IMapper mapper,
                           IArticleService articleService,
                           IArticleVersionService articleVersionService,
                           IArticleAttachmentService articleAttachmentService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _articleService = articleService;
        _articleVersionService = articleVersionService;
        _articleAttachmentService = articleAttachmentService;
    }

    public IActionResult Index()
    {
        var articles = _articleService.GetAllArticles();
        return View(articles);
    }

    public IActionResult Details(int id)
    {
        var article = _articleService.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        var versions = _articleVersionService.GetArticleVersions(id);
        var viewModel = new ArticleDetailViewModel
        {
            Article = article,
            Versions = versions
        };
        
        return View("Article", viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ArticleViewModel());
    }

    [HttpPost]
    public IActionResult Create([FromForm] ArticleViewModel model, List<IFormFile> attachments)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join(", ", errors) });
        }

        try
        {
            var userId = GetCurrentUserId();
            var articleId = _articleService.CreateArticle(model, userId);

            // Process attachments if any
            if (attachments != null && attachments.Any())
            {
                foreach (var file in attachments)
                {
                    _articleAttachmentService.AddAttachment(file, articleId, userId);
                }
            }

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var article = _articleService.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }
        return View(article);
    }

    [HttpPost]
    public IActionResult Edit(ArticleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return Json(new { success = false, message = string.Join(", ", errors) });
        }

        try
        {
            var userId = GetCurrentUserId();
            var originalArticle = _articleService.GetArticleById(model.ArticleId);
            
            _articleVersionService.CreateArticleVersion(
                model.ArticleId, 
                originalArticle.Title, 
                originalArticle.Content, 
                userId);

            _articleService.UpdateArticle(model, userId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpDelete]
    [Route("Article/Delete/{id}")]
    public IActionResult Delete(int id)
    {
        try 
        {
            var article = _articleService.GetArticleById(id);
            _articleService.DeleteArticle(id);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }
        throw new UnauthorizedAccessException("User is not authenticated");
    }
}