using System.Collections.Generic;
using ASI.Basecode.Services.ServiceModels;

public class HomeViewModel
{
    public IEnumerable<TicketViewModel> Tickets { get; set; }
    public IEnumerable<ArticleViewModel> Articles { get; set; }
}