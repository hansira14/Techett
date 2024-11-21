namespace ASI.Basecode.Services;

public interface IUserAuthorizationService
{
    bool CanModifyUser(string targetUserRole);
    bool CanAssignRole(string roleToAssign);
    bool CanDeleteComment(int commentUserId);
    bool CanModifyTicket(int ticketCreatorId, int? ticketId = null);
    bool CanDeleteTicket(int ticketCreatorId);
    bool CanUploadAttachment(int ticketCreatorId);
    bool CanDeleteAttachment(int ticketCreatorId);
}
