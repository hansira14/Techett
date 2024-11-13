using ASI.Basecode.Services;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    [Authorize]
    public class AttachmentController : ControllerBase<AttachmentController>
    {
        private readonly IAttachmentService _attachmentService;
        private readonly ITicketService _ticketService;
        private readonly IUserAuthorizationService _userAuthorizationService;
        public AttachmentController(IHttpContextAccessor httpContextAccessor,
                                  ILoggerFactory loggerFactory,
                                  IConfiguration configuration,
                                  IMapper mapper,
                                  IAttachmentService attachmentService,
                                  IUserAuthorizationService userAuthorizationService,
                                  ITicketService ticketService) 
            : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _attachmentService = attachmentService;
            _userAuthorizationService = userAuthorizationService;
            _ticketService = ticketService;
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file, int ticketId)
        {
            try
            {
                var ticket = _ticketService.GetTicketById(ticketId);
                if (!_userAuthorizationService.CanUploadAttachment(ticket.CreatedBy))
                {
                    return Json(new { success = false, message = "You don't have permission to upload attachments" });
                }
                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file uploaded" });
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
                var attachment = _attachmentService.GetAttachmentById(attachmentId);
                if (!_userAuthorizationService.CanDeleteAttachment(attachment.UploadedBy))
                {
                    return Json(new { success = false, message = "You don't have permission to delete this attachment" });
                }   
                _attachmentService.DeleteAttachment(attachmentId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("files/{*filePath}")]
        public IActionResult GetFile(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_configuration["FileServer:UploadPath"], filePath);
                
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound();
                }

                // Get content type based on file extension
                var contentType = Path.GetExtension(fullPath).ToLowerInvariant() switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".pdf" => "application/pdf",
                    ".doc" or ".docx" => "application/msword",
                    ".xls" or ".xlsx" => "application/vnd.ms-excel",
                    ".txt" => "text/plain",
                    _ => "application/octet-stream",
                };

                // Return the file with the correct content type
                return PhysicalFile(fullPath, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving file: {FilePath}", filePath);
                return StatusCode(500, "Error retrieving file");
            }
        }
    }
}