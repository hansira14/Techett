using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Notification
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

        public virtual User Agent { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual User User { get; set; }
    }
}
