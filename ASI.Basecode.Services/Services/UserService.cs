using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Security.Claims;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUserRepository repository,
            IFeedbackRepository feedbackRepository,
            IAssignmentRepository assignmentRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _feedbackRepository = feedbackRepository;
            _assignmentRepository = assignmentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public LoginResult AuthenticateUser(string email, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers()
                .FirstOrDefault(x => x.Email == email && 
                                    x.Password == passwordKey && 
                                    x.IsActive == true);
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

                var defaultPreference = new Preference
                {
                    UserId = user.UserId,
                    IsEmailingOn = false,
                    ViewType = "List"
                };
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }

        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var isSuperAdmin = _httpContextAccessor.HttpContext?.User.Claims
                .Any(c => c.Type == ClaimTypes.Role && 
                          c.Value.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase)) ?? false;
                  
            var users = _repository.GetUsers(isSuperAdmin);
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
                user.IsActive = false;
                user.UpdatedOn = DateTime.Now;
                _repository.UpdateUser(user);
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
                ProfilePictureUrl = string.IsNullOrEmpty(user.ProfilePicUrl) ? null : user.ProfilePicUrl,
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
                    ProfilePictureUrl = agent?.ProfilePicUrl ?? null,
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

        public void UpdateProfilePicture(int userId, string profilePictureUrl)
        {
            var user = _repository.GetById(userId);
            if (user != null)
            {
                user.ProfilePicUrl = profilePictureUrl;
                user.UpdatedOn = DateTime.Now;
                _repository.UpdateUser(user);
            }
        }

        public PaginatedUsersViewModel GetPaginatedUsers(string searchTerm, int page, int pageSize)
        {
            var query = _repository.GetUsers().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(u => 
                    u.Fname.ToLower().Contains(searchTerm) ||
                    u.Lname.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.Role.ToLower().Contains(searchTerm)
                );
            }

            var totalUsers = query.Count();
            var users = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserViewModel
                {
                    UserId = u.UserId,
                    Fname = u.Fname,
                    Lname = u.Lname,
                    Email = u.Email,
                    Role = u.Role,
                    TeamId = u.TeamId,
                    IsActive = u.IsActive ?? true
                })
                .ToList();

            return new PaginatedUsersViewModel
            {
                Users = users,
                TotalUsers = totalUsers,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }
}
