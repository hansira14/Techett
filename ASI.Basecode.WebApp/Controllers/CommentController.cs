using System;
using System.Linq;
using ASI.Basecode.Services;
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
    private readonly IUserAuthorizationService _userAuthorizationService;

    public CommentController(IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IMapper mapper,
        ICommentService commentService,
        IUserAuthorizationService userAuthorizationService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _commentService = commentService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet]
    public IActionResult GetTicketComments(int ticketId)
    {
        var comments = _commentService.GetTicketComments(ticketId);
        return Json(comments);
    }

    [HttpPost]
    public IActionResult AddComment(CommentViewModel model)
    {
        try
        {
            model.UserId = GetCurrentUserId();
            _commentService.AddComment(model);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult DeleteComment(int commentId)
    {
        try
        {
            var comment = _commentService.GetCommentById(commentId);
            if (!_userAuthorizationService.CanDeleteComment(comment.UserId))
            {
                return Json(new { success = false, message = "You don't have permission to delete this comment" });
            }

            _commentService.DeleteComment(commentId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }
        throw new UnauthorizedAccessException("User is not authenticated");
    }
}