using System;

namespace ASI.Basecode.Services.ServiceModels
{
    public class AgentViewModel
    {
        public int UserId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public int TicketsResolved { get; set; }
        public int TotalTicketsAssigned { get; set; }
        public double? AverageResolveTime { get; set; }
        public double? Rating { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
} 