using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ASI.Basecode.Services.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthorizationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
    }
} 