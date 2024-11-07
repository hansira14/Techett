using System;

public class CommentViewModel
{
    public int CommentId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Comment { get; set; }
    public DateTime CommentedOn { get; set; }
} 