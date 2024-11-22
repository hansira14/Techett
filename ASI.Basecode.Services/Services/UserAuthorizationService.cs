using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Data.Interfaces;
namespace ASI.Basecode.Services.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthorizationService(IHttpContextAccessor httpContextAccessor, IAssignmentRepository assignmentRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _assignmentRepository = assignmentRepository;
        }

        public bool CanModifyUser(string targetUserRole)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            if (currentUser == null) return false;

            var userRole = currentUser.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // SuperAdmin can modify all users
            if (userRole?.Equals(Roles.SuperAdmin, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            // Admin can only modify regular users and agents
            if (userRole?.Equals(Roles.Admin, StringComparison.OrdinalIgnoreCase) == true)
            {
                return targetUserRole.Equals(Roles.User, StringComparison.OrdinalIgnoreCase) ||
                       targetUserRole.Equals(Roles.Agent, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public bool CanAssignRole(string roleToAssign)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            if (currentUser == null) return false;

            var userRole = currentUser.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // SuperAdmin can assign any role
            if (userRole?.Equals(Roles.SuperAdmin, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            // Admin can only assign regular user and agent roles
            if (userRole?.Equals(Roles.Admin, StringComparison.OrdinalIgnoreCase) == true)
            {
                return roleToAssign.Equals(Roles.User, StringComparison.OrdinalIgnoreCase) ||
                       roleToAssign.Equals(Roles.Agent, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public bool CanDeleteComment(int commentUserId)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            if (currentUser == null) return false;

            var userIdClaim = currentUser.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
                return false;

            return currentUserId == commentUserId;
        }

        public bool CanModifyTicket(int ticketCreatorId, int? ticketId = null)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            if (currentUser == null) return false;

            var userRole = currentUser.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userIdClaim = currentUser.Claims
                .FirstOrDefault(c => c.Type == "UserId");

            // SuperAdmin and Admin can modify all tickets
            if (userRole?.Equals(Roles.SuperAdmin, StringComparison.OrdinalIgnoreCase) == true ||
                userRole?.Equals(Roles.Admin, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                // Check if user is the creator
                if (currentUserId == ticketCreatorId) return true;

                // Check if user is the assigned agent
                if (ticketId.HasValue && userRole?.Equals(Roles.Agent, StringComparison.OrdinalIgnoreCase) == true)
                {
                    var assignment = _assignmentRepository.GetAssignmentByTicketId(ticketId.Value);
                    return assignment?.AssignedTo == currentUserId;
                }
            }

            return false;
        }
        public bool CanUploadAttachment(int ticketCreatorId)
        {
            return CanModifyTicket(ticketCreatorId);
        }
        public bool CanDeleteAttachment(int ticketCreatorId)
        {
            return CanModifyTicket(ticketCreatorId);
        }   
        public bool CanDeleteTicket(int ticketCreatorId)
        {
            return CanModifyTicket(ticketCreatorId);
        }

        public bool CanModifyTicketField(int ticketCreatorId, int? ticketId, string fieldName)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            if (currentUser == null) return false;

            var userRole = currentUser.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userIdClaim = currentUser.Claims
                .FirstOrDefault(c => c.Type == "UserId");

            // SuperAdmin and Admin can modify all fields
            if (userRole?.Equals(Roles.SuperAdmin, StringComparison.OrdinalIgnoreCase) == true ||
                userRole?.Equals(Roles.Admin, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                // Regular users (creators) can only modify title and content
                if (currentUserId == ticketCreatorId)
                {
                    return fieldName.Equals("Title", StringComparison.OrdinalIgnoreCase) ||
                           fieldName.Equals("Content", StringComparison.OrdinalIgnoreCase);
                }

                // Agents can modify status, priority, and category if assigned
                if (ticketId.HasValue && userRole?.Equals(Roles.Agent, StringComparison.OrdinalIgnoreCase) == true)
                {
                    var assignment = _assignmentRepository.GetAssignmentByTicketId(ticketId.Value);
                    if (assignment?.AssignedTo == currentUserId)
                    {
                        // Allow agents to change status to resolved
                        if (fieldName.Equals("Status", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                        
                        return fieldName.Equals("Priority", StringComparison.OrdinalIgnoreCase) ||
                               fieldName.Equals("Category", StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            return false;
        }
    }
} 