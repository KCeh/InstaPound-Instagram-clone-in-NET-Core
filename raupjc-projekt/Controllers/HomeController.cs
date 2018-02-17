using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using raupjc_projekt.Models;
using raupjc_projekt.Models.AlbumViewModels;
using raupjc_projekt.Models.FavoriteViewModels;

namespace raupjc_projekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMySqlRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IMySqlRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            //provjeri ako je sve dobro preneseno, like, feature itd
            var user = await _userManager.GetUserAsync(HttpContext.User);
            IndexViewModelHome model = new IndexViewModelHome();
            if (user != null)
            {
                List<Photo> photosFromSubscribers = await _repository.GetPhotosFromSubscribedUsersAsync(user.Id);
                foreach (Photo photo in photosFromSubscribers)
                {
                    User owner = await _repository.GetUserId(photo.Id);
                    model.Photos.Add(new PhotoFavViewModel(photo, owner));
                }
            }
           
            //featured dodat
            return View(model);
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
