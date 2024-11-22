using System.Collections.Generic;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces;

public interface IUserService
{
    LoginResult AuthenticateUser(string email, string password, ref User user);
    IEnumerable<UserViewModel> GetAllUsers();
    UserViewModel GetUserById(int id);
    void AddUser(UserViewModel model, int adminId);
    void UpdateUser(UserViewModel model, int adminId);
    void DeleteUser(int id);
    UserProfileViewModel GetUserProfile(int userId);
    IEnumerable<AgentViewModel> GetAllAgents();
    void UpdateProfilePicture(int userId, string profilePictureUrl);
    PaginatedUsersViewModel GetPaginatedUsers(string searchTerm, int page, int pageSize);
}
