using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Assignment
    {
        public int AssignmentId { get; set; }
        public int TicketId { get; set; }
        public int AssignedTo { get; set; }
        public int AssignedBy { get; set; }
        public DateTime AssignedOn { get; set; }
        public DateTime? ResolvedOn { get; set; }

        public virtual User AssignedByNavigation { get; set; }
        public virtual User AssignedToNavigation { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
