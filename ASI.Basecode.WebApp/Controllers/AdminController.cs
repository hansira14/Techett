﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Assign()
        {
            return View();
        }

        public IActionResult Tickets()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
        public IActionResult Tools()
        {
            return View();
        }

        public async Task<IActionResult> SignOutUser()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
