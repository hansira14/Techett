using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers;

public class AttachmentController : ControllerBase<AttachmentController>
{
    private readonly IAttachmentService _attachmentService;
    public AttachmentController(IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper,
                                IAttachmentService attachmentService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _attachmentService = attachmentService;
    }
    public IActionResult Index()
    {
        return View();
    }   
}