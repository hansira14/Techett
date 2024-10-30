using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserController : ControllerBase<HomeController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public UserController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {

        }

        /// <summary>
        /// Returns Home View.
        /// </summary>
        /// <returns> Home View </returns>
        public IActionResult UserHome()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserIndex()
        {
            return View();
        }
        public IActionResult UserSettings()
        {
            return View();
        }

    }
}
