using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class CommentController : ControllerBase<CommentController>
{
    private readonly ICommentService _commentService;
    public CommentController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        ICommentService commentService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _commentService = commentService;
    }
    public IActionResult Index()
    {
        return View();
    }
}