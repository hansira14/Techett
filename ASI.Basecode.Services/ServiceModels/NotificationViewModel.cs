using System;

namespace ASI.Basecode.Services.ServiceModels
{
    public class NotificationViewModel
    {
        public int NotifId { get; set; }
        public int TicketId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentOn { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public int AgentId { get; set; }
        public string UserName { get; set; }
        public string AgentName { get; set; }
        public string TicketTitle { get; set; }
    }
}