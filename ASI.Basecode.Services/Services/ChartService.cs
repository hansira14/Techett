using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ASI.Basecode.Services
{
    public class ChartService : IChartService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IUpdateRepository _ticketUpdateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChartService(
            ITicketRepository ticketRepository, 
            IArticleRepository articleRepository,
            IUpdateRepository ticketUpdateRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _ticketRepository = ticketRepository;
            _articleRepository = articleRepository;
            _ticketUpdateRepository = ticketUpdateRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private (string role, int userId) GetCurrentUserInfo()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userRole = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            
            if (string.IsNullOrEmpty(userRole) || !int.TryParse(userIdClaim, out int userId))
            {
                return (null, 0);
            }
            
            return (userRole, userId);
        }

        public Dictionary<string, int> GetTicketDistributionByPriority()
        {
            var (role, userId) = GetCurrentUserInfo();
            if (role == null) return new Dictionary<string, int>();

            var tickets = _ticketRepository.GetAllTickets(role, userId).ToList();
            return tickets
                .GroupBy(t => t.Priority)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key switch { 1 => "Low", 2 => "Medium", 3 => "High", 4 => "Critical", _ => "Unknown" },
                    g => g.Count()
                );
        }

        public Dictionary<string, int> GetTicketStatusDistribution()
        {
            var (role, userId) = GetCurrentUserInfo();
            if (role == null) return new Dictionary<string, int>();

            var tickets = _ticketRepository.GetAllTickets(role, userId).ToList();
            return tickets
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<string, List<int>> GetTicketTrends(int days = 7)
        {
            var (role, userId) = GetCurrentUserInfo();
            if (role == null) return new Dictionary<string, List<int>>();

            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-(days - 1));
            
            var tickets = _ticketRepository.GetAllTickets(role, userId).ToList();
            var updates = _ticketUpdateRepository.GetAllUpdates()
                .OrderBy(u => u.UpdatedOn)
                .ToList();

            var trends = new Dictionary<string, List<int>>();
            var statuses = new[] { "Open", "Resolved", "In Progress" };

            foreach (var status in statuses)
            {
                var statusCounts = new List<int>();
                
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    int count;
                    if (status == "Resolved")
                    {
                        // Count tickets that were resolved on this specific date
                        count = tickets.Count(t => t.ResolvedOn?.Date == date);
                    }
                    else if (status == "In Progress")
                    {
                        // Get tickets that were in progress at the end of this date
                        var ticketsInProgress = updates
                            .Where(u => u.UpdatedOn.Date <= date)
                            .GroupBy(u => u.TicketId)
                            .Where(g => g.OrderByDescending(u => u.UpdatedOn)
                                        .First().Status == "In Progress")
                            .Count();
                        count = ticketsInProgress;
                    }
                    else // Open tickets
                    {
                        // Get total tickets created before this date
                        var totalTickets = tickets.Count(t => 
                            (t.CreatedOn.Month < date.Month && t.CreatedOn.Year <= date.Year) ||
                            (t.CreatedOn.Month == date.Month && t.CreatedOn.Day < date.Day && t.CreatedOn.Year <= date.Year)
                        );
                        
                        // Get tickets that were resolved before this date
                        var resolvedTickets = tickets.Count(t => 
                            t.ResolvedOn.HasValue && (
                                (t.ResolvedOn.Value.Month < date.Month && t.ResolvedOn.Value.Year <= date.Year) ||
                                (t.ResolvedOn.Value.Month == date.Month && t.ResolvedOn.Value.Day < date.Day && t.ResolvedOn.Value.Year <= date.Year)
                            )
                        );
                        
                        // Get tickets that were in progress at the end of this date
                        var inProgressTickets = updates
                            .Where(u => u.UpdatedOn.Date <= date)
                            .GroupBy(u => u.TicketId)
                            .Count(g => g.OrderByDescending(u => u.UpdatedOn)
                                        .First().Status == "In Progress");
                        
                        // Open tickets = Total - Resolved - InProgress
                        count = totalTickets - resolvedTickets - inProgressTickets;
                    }
                    
                    statusCounts.Add(count);
                }
                
                trends[status] = statusCounts;
            }

            return trends;
        }

        public Dictionary<string, object> GetTicketStatistics()
        {
            var (role, userId) = GetCurrentUserInfo();
            if (role == null) return new Dictionary<string, object>();

            var tickets = _ticketRepository.GetAllTickets(role, userId).ToList();
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