﻿using System;
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
    public class ExploreController : Controller
    {
        private readonly IMySqlRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ExploreController(IMySqlRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
           
            List<Album> albums = await _repository.GetAllAlbumsAsync(user.Id);
            Models.AlbumViewModels.IndexViewModel model = new IndexViewModel();
            foreach (Album album in albums)
            {
                AlbumViewModel viewModel = new AlbumViewModel(album.Id, album.DateCreated, album.Owner, album.Name);
                List<Photo> photos = await _repository.GetPhotosAsync(album.Id);
                if (photos.Count > 0)
                {
                    viewModel.ThumbnailImage = photos.First().ThumbnailImage;
                }
                model.Albums.Add(viewModel);

            }
            return View(model);
        }

        public async Task<IActionResult> ShowAlbumPhotos(Guid id)
        {
            Album album = await _repository.GetAlbumAsync(id);
            AlbumViewModel model = new AlbumViewModel(album.Id, album.DateCreated, album.Owner, album.Name);
            model.Photos = await _repository.GetPhotosAsync(model.Id);
            return View("OtherUserAlbum", model);
        }

        public async Task<IActionResult> SubscribeToUser(string ownerId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.SubscribeToUserAsync(user.Id, ownerId);
            return RedirectToAction("Index", "Home");
        }


    }
}