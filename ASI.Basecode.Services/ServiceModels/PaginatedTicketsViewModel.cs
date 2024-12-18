using System.Collections.Generic;
using ASI.Basecode.Services.ServiceModels;

public class PaginatedTicketsViewModel
{
    public IEnumerable<TicketViewModel> Tickets { get; set; }
    public int TotalTickets { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public string SearchTerm { get; set; }
    public string[] SelectedCategories { get; set; }
    public string[] SelectedPriorities { get; set; }
    public string SortColumn { get; set; }
    public string SortDirection { get; set; }
}
