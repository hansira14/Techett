using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ASI.Basecode.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChartController : ControllerBase<ChartController>
    {
        private readonly IChartService _chartService;

        public ChartController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,
            IChartService chartService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _chartService = chartService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetChartData")]
        public IActionResult GetChartData()
        {
            var priorityData = _chartService.GetTicketDistributionByPriority();
            var statusData = _chartService.GetTicketStatusDistribution();
            var trendData = _chartService.GetTicketTrends();
            var statistics = _chartService.GetTicketStatistics();
            var articleTrends = _chartService.GetArticleCreationTrends();

            return Json(new
            {
                priorityDistribution = priorityData,
                statusDistribution = statusData,
                ticketTrends = trendData,
                statistics = statistics,
                articleTrends = articleTrends
            });
        }

        [HttpGet("GetUserTicketCounts")]
        public IActionResult GetUserTicketCounts()
        {
            var counts = _chartService.GetUserTicketCounts();
            return Json(counts);
        }
    }
} 