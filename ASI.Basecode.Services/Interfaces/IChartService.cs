using System.Collections.Generic;

public interface IChartService
{
    Dictionary<string, int> GetTicketDistributionByPriority();
    Dictionary<string, int> GetTicketStatusDistribution();
    Dictionary<string, List<int>> GetTicketTrends(int days = 7);
    Dictionary<string, object> GetTicketStatistics();
    List<int> GetArticleCreationTrends(int days = 7);
    Dictionary<string, int> GetUserTicketCounts();
} 