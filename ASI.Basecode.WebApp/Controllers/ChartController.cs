using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using Microsoft.AspNetCore.Http;

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

        [HttpGet("GetChartData")]
        public IActionResult GetChartData()
        {
            var priorityData = _chartService.GetTicketDistributionByPriority();
            var statusData = _chartService.GetTicketStatusDistribution();
            var trendData = _chartService.GetTicketTrends();
            var statistics = _chartService.GetTicketStatistics();

            return Json(new
            {
                priorityDistribution = priorityData,
                statusDistribution = statusData,
                ticketTrends = trendData,
                statistics = statistics
            });
        }
    }
} 