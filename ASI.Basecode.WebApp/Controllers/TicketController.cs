using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;
using ASI.Basecode.Services;

namespace ASI.Basecode.WebApp.Controllers
{
    public class TicketController : ControllerBase<TicketController>
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly IAttachmentService _attachmentService;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public TicketController(IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,
            ITicketService ticketService,
            IUserService userService,
            IAttachmentService attachmentService,
            IUserAuthorizationService userAuthorizationService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _ticketService = ticketService;
            _userService = userService;
            _attachmentService = attachmentService;
            _userAuthorizationService = userAuthorizationService;
        }

        public IActionResult Tickets()
        {
            var tickets = _ticketService.GetAllTickets();
            return View("Tickets", tickets);
        }

        public IActionResult Details(int id)
        {
            var ticket = _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        [HttpPost]
        public IActionResult CreateTicket([FromForm] TicketViewModel model, List<IFormFile> attachments)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid model state" });
            }

            try
            {
                var userId = GetCurrentUserId();
                var ticketId = _ticketService.CreateTicket(model, userId);

                // Process attachments if any
                if (attachments != null && attachments.Any())
                {
                    foreach (var file in attachments)
                    {
                        _attachmentService.AddAttachment(file, ticketId, userId);
                    }
                }

                return Json(new { success = true });
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { success = false, message = "User not authenticated" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EditTicket(TicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid model state" });
            }

            try
            {
                var ticket = _ticketService.GetTicketById(model.TicketId);
                if (ticket == null)
                {
                    return Json(new { success = false, message = "Ticket not found" });
                }

                if (!_userAuthorizationService.CanModifyTicket(ticket.CreatedBy))
                {
                    return Json(new { success = false, message = "You don't have permission to modify this ticket" });
                }

                _ticketService.UpdateTicket(model, GetCurrentUserId());
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteTicket(int id)
        {
            try
            {
                var ticket = _ticketService.GetTicketById(id);
                if (ticket == null)
                {
                    return Json(new { success = false, message = "Ticket not found" });
                }

                if (!_userAuthorizationService.CanDeleteTicket(ticket.CreatedBy))
                {
                    return Json(new { success = false, message = "You don't have permission to delete this ticket" });
                }

                _ticketService.DeleteTicket(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            // Get the claim containing the user ID
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User is not authenticated");
        }
    }
}