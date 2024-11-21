using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Data.Models;
using AutoMapper;
using System;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }

        public void AddFeedback(FeedbackViewModel model)
        {
            var feedback = _mapper.Map<Feedback>(model);
            feedback.CreatedOn = DateTime.Now;
            _feedbackRepository.AddFeedback(feedback);
        }

        public FeedbackViewModel GetFeedbackByTicketId(int ticketId)
        {
            var feedback = _feedbackRepository.GetAllFeedbacks()
                .FirstOrDefault(f => f.TicketId == ticketId);
            return _mapper.Map<FeedbackViewModel>(feedback);
        }

        public bool HasUserSubmittedFeedback(int ticketId, int userId)
        {
            return _feedbackRepository.GetAllFeedbacks()
                .Any(f => f.TicketId == ticketId && f.UserId == userId);
        }
    }
}