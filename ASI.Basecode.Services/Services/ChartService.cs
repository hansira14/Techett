using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;


namespace ASI.Basecode.Services
{
    public class ChartService : IChartService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IArticleRepository _articleRepository;

        public ChartService(ITicketRepository ticketRepository, IArticleRepository articleRepository)
        {
            _ticketRepository = ticketRepository;
            _articleRepository = articleRepository;
        }

        public Dictionary<string, int> GetTicketDistributionByPriority()
        {
            var tickets = _ticketRepository.GetAllTickets().ToList();
            return tickets
                .GroupBy(t => t.Priority)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key switch { 1 => "Low", 2 => "Medium", 3 => "High", _ => "Unknown" },
                    g => g.Count()
                );
        }

        public Dictionary<string, int> GetTicketStatusDistribution()
        {
            var tickets = _ticketRepository.GetAllTickets().ToList();
            return tickets
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<string, List<int>> GetTicketTrends(int days = 7)
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-days);
            var tickets = _ticketRepository.GetAllTickets()
                .Where(t => t.CreatedOn >= startDate)
                .ToList();

            var trends = new Dictionary<string, List<int>>();
            var statuses = new[] { "Open", "Resolved", "In Progress" };

            foreach (var status in statuses)
            {
                var statusCounts = new List<int>();
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    var count = tickets.Count(t => 
                        t.Status == status && 
                        t.CreatedOn.Date == date.Date);
                    statusCounts.Add(count);
                }
                trends[status] = statusCounts;
            }

            return trends;
        }

        public Dictionary<string, object> GetTicketStatistics()
        {
            var tickets = _ticketRepository.GetAllTickets().ToList();
            var now = DateTime.Now;
            var lastMonth = now.AddMonths(-1);

            var currentTotal = tickets.Count;
            var lastMonthTotal = tickets.Count(t => t.CreatedOn <= lastMonth);
            
            var totalChange = lastMonthTotal > 0 
                ? ((currentTotal - lastMonthTotal) * 100.0 / lastMonthTotal)
                : 100;

            return new Dictionary<string, object>
            {
                ["totalTickets"] = new
                {
                    count = currentTotal,
                    change = Math.Round(totalChange, 1)
                },
                ["openTickets"] = new
                {
                    count = tickets.Count(t => t.Status == "Open"),
                    change = CalculatePercentageChange(tickets, "Open", lastMonth)
                },
                ["resolvedTickets"] = new
                {
                    count = tickets.Count(t => t.Status == "Resolved"),
                    change = CalculatePercentageChange(tickets, "Resolved", lastMonth)
                },
                ["pendingTickets"] = new
                {
                    count = tickets.Count(t => t.Status == "In Progress"),
                    change = CalculatePercentageChange(tickets, "In Progress", lastMonth)
                }
            };
        }

        private double CalculatePercentageChange(List<Ticket> tickets, string status, DateTime lastMonth)
        {
            var currentCount = tickets.Count(t => t.Status == status);
            var lastMonthCount = tickets.Count(t => t.Status == status && t.CreatedOn <= lastMonth);
            
            return lastMonthCount > 0 
                ? Math.Round(((currentCount - lastMonthCount) * 100.0 / lastMonthCount), 1)
                : 100;
        }

        public List<int> GetArticleCreationTrends(int days = 7)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-(days - 1));
            
            var articles = _articleRepository.GetAllArticles()
                .Where(a => a.CreatedOn >= startDate && a.IsDeleted == false)
                .ToList();

            var trendData = new List<int>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var count = articles.Count(a => a.CreatedOn.Date == date);
                trendData.Add(count);
            }

            return trendData;
        }
    }
} 