using System;

public class UpdateViewModel
{
    public int UpdateId { get; set; }
    public int TicketId { get; set; }
    public string Status { get; set; }
    public int? Priority { get; set; }
    public string Message { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int UpdatedBy { get; set; }
    public string UpdatedByName { get; set; }
} 