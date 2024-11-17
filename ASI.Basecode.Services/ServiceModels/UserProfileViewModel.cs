using System.Collections.Generic;

public class UserProfileViewModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string ProfilePictureUrl { get; set; }
    public int TotalTicketsAssigned { get; set; }
    public int TotalTicketsSolved { get; set; }
    public int PendingTickets { get; set; }
    public double? AverageResolveTime { get; set; }
    public List<FeedbackViewModel> Feedbacks { get; set; }
}
