using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public int AgentId { get; set; }
        public byte Rating { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual User Agent { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual User User { get; set; }
    }
}
