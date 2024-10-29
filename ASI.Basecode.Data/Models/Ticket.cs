using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            Assignments = new HashSet<Assignment>();
            Attachments = new HashSet<Attachment>();
            Comments = new HashSet<Comment>();
            Feedbacks = new HashSet<Feedback>();
            Notifications = new HashSet<Notification>();
            Updates = new HashSet<Update>();
        }

        public int TicketId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DueDate { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Update> Updates { get; set; }
    }
}
