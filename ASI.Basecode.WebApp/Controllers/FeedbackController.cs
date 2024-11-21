using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ASI.Basecode.WebApp.Controllers
{
    public class FeedbackController : ControllerBase<FeedbackController>
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ITicketService _ticketService;

        public FeedbackController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,
            IFeedbackService feedbackService,
            ITicketService ticketService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _feedbackService = feedbackService;
            _ticketService = ticketService;
        }

        [HttpPost]
        public IActionResult SubmitFeedback([FromBody] FeedbackViewModel model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid feedback data" });

                var userId = GetCurrentUserId();
                var ticket = _ticketService.GetTicketById(model.TicketId);

                if (ticket == null)
                    return Json(new { success = false, message = "Ticket not found" });

                if (ticket.CreatedBy != userId)
                    return Json(new { success = false, message = "Only ticket creator can submit feedback" });

                if (_feedbackService.HasUserSubmittedFeedback(model.TicketId, userId))
                    return Json(new { success = false, message = "Feedback already submitted" });

                model.UserId = userId;
                model.AgentId = ticket.AssignedToId ?? 0;
                model.CreatedOn = DateTime.Now;

                if (model.AgentId == 0)
                    return Json(new { success = false, message = "No agent assigned to this ticket" });

                _feedbackService.AddFeedback(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult CheckFeedbackStatus(int ticketId)
        {
            var userId = GetCurrentUserId();
            var hasFeedback = _feedbackService.HasUserSubmittedFeedback(ticketId, userId);
            return Json(new { hasFeedback });
        }
    }
}