using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Update
    {
        public int UpdateId { get; set; }
        public int TicketId { get; set; }
        public string Status { get; set; }
        public int? Priority { get; set; }
        public string Message { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
