using Microsoft.AspNetCore.Mvc;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserHome()
        {
            return View();
        }
    }
}
