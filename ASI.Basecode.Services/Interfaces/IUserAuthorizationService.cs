namespace ASI.Basecode.Services;

public interface IUserAuthorizationService
{
    bool CanModifyUser(string targetUserRole);
    bool CanAssignRole(string roleToAssign);
    bool CanDeleteComment(int commentUserId);
    bool CanModifyTicket(int ticketCreatorId);
    bool CanDeleteTicket(int ticketCreatorId);
}