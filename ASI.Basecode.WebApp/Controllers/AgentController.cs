using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AgentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
