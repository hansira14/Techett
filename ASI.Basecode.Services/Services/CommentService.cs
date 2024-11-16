using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using AutoMapper;

namespace ASI.Basecode.Services.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentService(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public IEnumerable<CommentViewModel> GetTicketComments(int ticketId)
    {
        var comments = _commentRepository.GetAllComments()
            .Where(c => c.TicketId == ticketId)
            .OrderBy(c => c.CommentedOn);
        return _mapper.Map<IEnumerable<CommentViewModel>>(comments);
    }

    public void AddComment(CommentViewModel model)
    {
        var comment = _mapper.Map<Comment>(model);
        comment.CommentedOn = DateTime.Now;
        _commentRepository.AddComment(comment);
    }

    public void DeleteComment(int commentId)
    {
        var comment = _commentRepository.GetCommentById(commentId);
        if (comment != null)
        {
            _commentRepository.DeleteComment(comment);
        }
    }

    public Comment GetCommentById(int commentId)
    {
        return _commentRepository.GetCommentById(commentId);
    }
}
