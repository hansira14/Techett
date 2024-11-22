using System.Collections.Generic;
using ASI.Basecode.Services.ServiceModels;

public class PaginatedUsersViewModel
{
    public IEnumerable<UserViewModel> Users { get; set; }
    public int TotalUsers { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}
