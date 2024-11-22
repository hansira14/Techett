using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class User
    {
        public User()
        {
            ArticleAttachments = new HashSet<ArticleAttachment>();
            ArticleVersions = new HashSet<ArticleVersion>();
            Articles = new HashSet<Article>();
            AssignmentAssignedByNavigations = new HashSet<Assignment>();
            AssignmentAssignedToNavigations = new HashSet<Assignment>();
            Attachments = new HashSet<Attachment>();
            Comments = new HashSet<Comment>();
            FeedbackAgents = new HashSet<Feedback>();
            FeedbackUsers = new HashSet<Feedback>();
            InverseCreatedByNavigation = new HashSet<User>();
            InverseUpdatedByNavigation = new HashSet<User>();
            NotificationAgents = new HashSet<Notification>();
            NotificationUsers = new HashSet<Notification>();
            Preferences = new HashSet<Preference>();
            Sessions = new HashSet<Session>();
            TicketCreatedByNavigations = new HashSet<Ticket>();
            TicketResolvedByNavigations = new HashSet<Ticket>();
            Updates = new HashSet<Update>();
        }

        public int UserId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? TeamId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }
        public string ProfilePicUrl { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Team Team { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<ArticleAttachment> ArticleAttachments { get; set; }
        public virtual ICollection<ArticleVersion> ArticleVersions { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Assignment> AssignmentAssignedByNavigations { get; set; }
        public virtual ICollection<Assignment> AssignmentAssignedToNavigations { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Feedback> FeedbackAgents { get; set; }
        public virtual ICollection<Feedback> FeedbackUsers { get; set; }
        public virtual ICollection<User> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<User> InverseUpdatedByNavigation { get; set; }
        public virtual ICollection<Notification> NotificationAgents { get; set; }
        public virtual ICollection<Notification> NotificationUsers { get; set; }
        public virtual ICollection<Preference> Preferences { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<Ticket> TicketCreatedByNavigations { get; set; }
        public virtual ICollection<Ticket> TicketResolvedByNavigations { get; set; }
        public virtual ICollection<Update> Updates { get; set; }
    }
}
