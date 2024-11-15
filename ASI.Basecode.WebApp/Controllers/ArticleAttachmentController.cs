using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class ArticleAttachmentController : ControllerBase<ArticleAttachmentController>
{
    private readonly IArticleAttachmentService _articleAttachmentService;

    public ArticleAttachmentController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        IArticleAttachmentService articleAttachmentService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _articleAttachmentService = articleAttachmentService;
    }
}