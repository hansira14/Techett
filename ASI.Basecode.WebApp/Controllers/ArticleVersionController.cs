using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
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
}