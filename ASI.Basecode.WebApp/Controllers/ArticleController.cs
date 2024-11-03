using System.Collections.Generic;
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

    public ArticleController(IHttpContextAccessor httpContextAccessor,
                           ILoggerFactory loggerFactory,
                           IConfiguration configuration,
                           IMapper mapper,
                           IArticleService articleService,
                           IArticleVersionService articleVersionService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _articleService = articleService;
        _articleVersionService = articleVersionService;
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
    public IActionResult Create(ArticleViewModel model)
    {
        if (ModelState.IsValid)
        {
            _articleService.CreateArticle(model, GetCurrentUserId());
            return RedirectToAction(nameof(Index));
        }
        return View(model);
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
        if (ModelState.IsValid)
        {
            var userId = GetCurrentUserId();
            // Create version before updating
            _articleVersionService.CreateArticleVersion(model.ArticleId, model.Title, model.Content, userId);
            // Update article
            _articleService.UpdateArticle(model, userId);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _articleService.DeleteArticle(id);
        return RedirectToAction(nameof(Index));
    }

    private int GetCurrentUserId()
    {
        // Implement getting current user ID from session/claims
        return 1; // Temporary return value
    }
}