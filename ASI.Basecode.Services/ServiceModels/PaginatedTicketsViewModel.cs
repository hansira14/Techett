using System.Collections.Generic;
using ASI.Basecode.Services.ServiceModels;

public class PaginatedTicketsViewModel
{
    public IEnumerable<TicketViewModel> Tickets { get; set; }
    public int TotalTickets { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public string SearchTerm { get; set; }
}
