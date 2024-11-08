using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AttachmentController : ControllerBase<AttachmentController>
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IHttpContextAccessor httpContextAccessor,
                                  ILoggerFactory loggerFactory,
                                  IConfiguration configuration,
                                  IMapper mapper,
                                  IAttachmentService attachmentService) 
            : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _attachmentService = attachmentService;
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file, int ticketId)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file uploaded" });

                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
                var attachment = _attachmentService.AddAttachment(file, ticketId, userId);
                return Json(new { success = true, attachment });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetTicketAttachments(int ticketId)
        {
            var attachments = _attachmentService.GetTicketAttachments(ticketId);
            return Json(attachments);
        }

        [HttpPost]
        public IActionResult Delete(int attachmentId)
        {
            try
            {
                _attachmentService.DeleteAttachment(attachmentId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}