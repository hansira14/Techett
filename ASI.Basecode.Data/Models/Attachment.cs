using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Attachment
    {
        public int AtachmentId { get; set; }
        public int TicketId { get; set; }
        public string Source { get; set; }
        public DateTime UploadedOn { get; set; }
        public int UploadedBy { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual User UploadedByNavigation { get; set; }
    }
}
