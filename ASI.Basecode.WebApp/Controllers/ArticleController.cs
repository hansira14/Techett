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

    public ArticleController(IHttpContextAccessor httpContextAccessor,
                           ILoggerFactory loggerFactory,
                           IConfiguration configuration,
                           IMapper mapper,
                           IArticleService articleService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _articleService = articleService;
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
        return View("Article", article);
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
        return View(article.Article);
    }

    [HttpPost]
    public IActionResult Edit(ArticleViewModel model)
    {
        if (ModelState.IsValid)
        {
            _articleService.UpdateArticle(model, GetCurrentUserId());
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