using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    [Authorize]
    public class ArticleAttachmentController : ControllerBase<ArticleAttachmentController>
    {
        private readonly IArticleAttachmentService _articleAttachmentService;
        private readonly IArticleService _articleService;

        public ArticleAttachmentController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,
            IArticleAttachmentService articleAttachmentService,
            IArticleService articleService) 
            : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _articleAttachmentService = articleAttachmentService;
            _articleService = articleService;
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file, int articleId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file uploaded" });
                
                var attachment = _articleAttachmentService.AddAttachment(file, articleId, userId);
                return Json(new { success = true, attachment });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetArticleAttachments(int articleId)
        {
            var attachments = _articleAttachmentService.GetArticleAttachments(articleId);
            return Json(attachments);
        }

        [HttpPost]
        public IActionResult Delete(int attachmentId)
        {
            try
            {
                _articleAttachmentService.DeleteAttachment(attachmentId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}