using System;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public int Priority { get; set; }

        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public int? ResolvedBy { get; set; }
        public string ResolvedByName { get; set; }
        public int? AssignedToId { get; set; }
        public string AssignedToName { get; set; }
    }
}
