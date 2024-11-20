using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository repository,
            IFeedbackRepository feedbackRepository,
            IAssignmentRepository assignmentRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _feedbackRepository = feedbackRepository;
            _assignmentRepository = assignmentRepository;
        }

        public LoginResult AuthenticateUser(string email, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().FirstOrDefault(x => x.Email == email &&
                                                              x.Password == passwordKey);
            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model, int adminID)
        {
            var user = new User();
            if (!_repository.UserExists(model.UserId))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedOn = DateTime.Now;
                user.CreatedBy = adminID;
                user.UpdatedBy = adminID;
                user.IsActive = true;

                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }

        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var users = _repository.GetUsers();
            return _mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        public UserViewModel GetUserById(int id)
        {
            var user = _repository.GetUsers().FirstOrDefault(x => x.UserId == id);
            return _mapper.Map<UserViewModel>(user);
        }

        public void UpdateUser(UserViewModel model, int adminId)
        {
            var user = _repository.GetUsers().FirstOrDefault(x => x.UserId == model.UserId);
            if (user != null)
            {
                user.Fname = model.Fname;
                user.Lname = model.Lname;
                user.Email = model.Email;
                user.Role = model.Role;
                user.UpdatedBy = adminId;
                user.UpdatedOn = DateTime.Now;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = PasswordManager.EncryptPassword(model.Password);
                }

                _repository.UpdateUser(user);
            }
            else
            {
                throw new InvalidDataException("User not found");
            }
        }

        public void DeleteUser(int id)
        {
            var user = _repository.GetUsers().FirstOrDefault(x => x.UserId == id);
            if (user != null)
            {
                _repository.DeleteUser(user);
            }
            else
            {
                throw new InvalidDataException("User not found");
            }
        }

        public UserProfileViewModel GetUserProfile(int userId)
        {
            var user = _repository.GetById(userId);
            if (user == null) return null;

            var assignments = _assignmentRepository.GetUserAssignments(userId);

            var profile = new UserProfileViewModel
            {
                Name = $"{user.Fname} {user.Lname}",
                Email = user.Email,
                Role = user.Role,
                ProfilePictureUrl = "https://placehold.co/150x150",
                TotalTicketsAssigned = assignments.Count(),
                TotalTicketsSolved = assignments.Count(a => a.Ticket.Status == "Resolved"),
                PendingTickets = assignments.Count(a => a.Ticket.Status != "Resolved"),
                AverageResolveTime = assignments
                    .Where(a => a.Ticket.Status == "Resolved" && a.Ticket.ResolvedOn.HasValue)
                    .AsEnumerable()
                    .DefaultIfEmpty()
                    .Average(a => a == null ? 0 : (a.Ticket.ResolvedOn.Value - a.AssignedOn).TotalHours),
                Feedbacks = _feedbackRepository.GetAllFeedbacks()
                    .Where(f => f.AgentId == userId)
                    .OrderByDescending(f => f.CreatedOn)
                    .Select(f => new FeedbackViewModel
                    {
                        Rating = f.Rating,
                        Remarks = f.Remarks,
                        CreatedOn = f.CreatedOn,
                        UserName = $"{f.User.Fname} {f.User.Lname}",
                        TicketTitle = f.Ticket.Title
                    })
                    .ToList()
            };

            return profile;
        }

        public IEnumerable<AgentViewModel> GetAllAgents()
        {
            var agents = _repository.GetUsers()
                .Where(u => u.Role == "Agent")
                .ToList();

            var agentViewModels = new List<AgentViewModel>();

            foreach (var agent in agents)
            {
                var assignments = _assignmentRepository.GetUserAssignments(agent.UserId)
                    .ToList();

                var feedbacks = _feedbackRepository.GetAllFeedbacks()
                    .Where(f => f.AgentId == agent.UserId);

                var agentViewModel = new AgentViewModel
                {
                    UserId = agent.UserId,
                    Fname = agent.Fname,
                    Lname = agent.Lname,
                    Email = agent.Email,
                    TicketsResolved = assignments.Count(a => a.Ticket.Status == "Resolved"),
                    TotalTicketsAssigned = assignments.Count,
                    AverageResolveTime = assignments
                        .Where(a => a.Ticket.Status == "Resolved" && a.Ticket.ResolvedOn.HasValue)
                        .Select(a => (a.Ticket.ResolvedOn.Value - a.AssignedOn).TotalHours)
                        .DefaultIfEmpty()
                        .Average(),
                    Rating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : null
                };

                agentViewModels.Add(agentViewModel);
            }

            return agentViewModels;
        }
    }
}
