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

        [HttpGet]
        public IActionResult Tickets(string searchTerm = "", int page = 1, int pageSize = 12)
        {
            var paginatedTickets = _ticketService.GetPaginatedTickets(searchTerm, page, pageSize);
            return View(paginatedTickets);
        }

        [HttpGet]
        public IActionResult SearchTickets(string searchTerm, int page = 1, int pageSize = 12, 
            string[] categories = null, int[] priorities = null, 
            string sortColumn = null, string sortDirection = "asc")
        {
            categories = categories?.Length == 0 ? null : categories;
            priorities = priorities?.Length == 0 ? null : priorities;

            var paginatedTickets = _ticketService.GetPaginatedTickets(
                searchTerm, 
                page, 
                pageSize, 
                categories, 
                priorities?.Select(p => p.ToString()).ToArray(),
                sortColumn,
                sortDirection
            );
            return Json(paginatedTickets);
        }

        public IActionResult Details(int id)
        {
            var ticket = _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }
            
            ViewBag.CurrentUserId = GetCurrentUserId();
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
                var existingTicket = _ticketService.GetTicketById(model.TicketId);
                if (existingTicket == null)
                {
                    return Json(new { success = false, message = "Ticket not found" });
                }

                if (existingTicket.Status == "Resolved")
                {
                    return Json(new { success = false, message = "Cannot edit resolved tickets" });
                }

                // Check field-level permissions
                if (model.Status != existingTicket.Status && 
                    !_userAuthorizationService.CanModifyTicketField(existingTicket.CreatedBy, existingTicket.TicketId, "Status"))
                {
                    return Json(new { success = false, message = "You don't have permission to modify the status" });
                }

                if (model.Priority != existingTicket.Priority && 
                    !_userAuthorizationService.CanModifyTicketField(existingTicket.CreatedBy, existingTicket.TicketId, "Priority"))
                {
                    return Json(new { success = false, message = "You don't have permission to modify the priority" });
                }

                if (model.Category != existingTicket.Category && 
                    !_userAuthorizationService.CanModifyTicketField(existingTicket.CreatedBy, existingTicket.TicketId, "Category"))
                {
                    return Json(new { success = false, message = "You don't have permission to modify the category" });
                }

                if ((model.Title != existingTicket.Title || model.Content != existingTicket.Content) && 
                    !_userAuthorizationService.CanModifyTicketField(existingTicket.CreatedBy, existingTicket.TicketId, "Title"))
                {
                    return Json(new { success = false, message = "You don't have permission to modify the title or content" });
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
    }
}