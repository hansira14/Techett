using System;

public class AssignmentViewModel
{
    public int AssignmentId { get; set; }
    public int TicketId { get; set; }
    public int AssignedTo { get; set; }
    public int AssignedBy { get; set; }
    public string AssignedToName { get; set; }
    public string AssignedByName { get; set; }
    public DateTime AssignedOn { get; set; }
}
