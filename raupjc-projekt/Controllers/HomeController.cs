using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using raupjc_projekt.Models;

namespace raupjc_projekt.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact page.";

            return View();
        }

        public IActionResult MyAlbums()
        {
            ViewData["Message"] = "View and manage your albums and photos";

            return RedirectToAction("Index", "Album");
        }

        public IActionResult Explore()
        {
            ViewData["Message"] = "Explore other users photos";

            return RedirectToAction("Index", "Explore");
        }

        public IActionResult MyFavorites()
        {
            ViewData["Message"] = "View your favorite photos";

            return RedirectToAction("Index", "Favorites");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
