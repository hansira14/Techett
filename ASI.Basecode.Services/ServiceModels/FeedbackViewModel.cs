using System;
using System.ComponentModel.DataAnnotations;

public class FeedbackViewModel
{
    public int FeedbackId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public int AgentId { get; set; }
    [Required]
    [Range(1, 5)]
    public byte Rating { get; set; }
    [Required]
    public string Remarks { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UserName { get; set; }
    public string AgentName { get; set; }
    public string TicketTitle { get; set; }
}