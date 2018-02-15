using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using raupjc_projekt.Models;
using raupjc_projekt.Models.AlbumViewModels;

namespace raupjc_projekt.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly IMySqlRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlbumController(IMySqlRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var modelUser = _repository.GetUser(user.Id);
            List<Album> albums =await _repository.GetMyAlbumsAsync(user.Id);

            Models.AlbumViewModels.IndexViewModel model = new IndexViewModel();
            foreach (Album album in albums)
            {
                model.Albums.Add(new AlbumViewModel(album.Id, album.DateCreated, modelUser,album.Name));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAlbumViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            User myUser = _repository.GetUser(user.Id);
            Album album = new Album(myUser, model.Name);

           // myUser.Albums.Add(album); BAZU popraviti
            await _repository.AddMyAlbumAsync(album);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            User myUser = _repository.GetUser(user.Id);
            await _repository.RemoveMyAlbumAsync(myUser, id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Rename(Guid id)
        {
            Album album = _repository.GetAlbum(id);
            RenameAlbumViewModel model=new RenameAlbumViewModel(id,album.Name);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Rename(RenameAlbumViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Rename", model);
            }
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            User myUser = _repository.GetUser(user.Id);
            Album album = _repository.GetAlbum(model.Id);
            album.Name = model.Name;
            await _repository.UpdateMyAlbumAsync(myUser,album);
            return RedirectToAction("Index");
        }

        public IActionResult ShowAlbumPhotos(AlbumViewModel album)
        {

            return View("Album", album);
        }


    }

}