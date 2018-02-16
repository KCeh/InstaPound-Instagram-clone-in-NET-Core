using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using raupjc_projekt.Models;
using raupjc_projekt.Models.FavoriteViewModels;

namespace raupjc_projekt.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IMySqlRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        public FavoritesController(IMySqlRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            FavoriteViewModel viewModel=new FavoriteViewModel();
            List<Photo> favorites= await _repository.GetFavoritePhotos(user.Id);
            foreach (Photo photo in favorites)
            {
                User owner = await _repository.GetUserId(photo.Id);
                viewModel.Photos.Add(new PhotoFavViewModel(photo,owner));
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Favorite(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.FavoritePhotoAsync(user.Id, id);
            return RedirectToAction("Index"); //popraviti
        }
    }
}