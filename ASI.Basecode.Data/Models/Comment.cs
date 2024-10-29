using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string Comment1 { get; set; }
        public DateTime CommentedOn { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual User User { get; set; }
    }
}
