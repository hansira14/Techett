using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
        public IActionResult Tickets()
        {
            return View();
        }

        public async Task<IActionResult> SignOutUser()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
