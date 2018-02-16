using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using raupjc_projekt.Models;
using raupjc_projekt.Models.AlbumViewModels;

namespace raupjc_projekt.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly IMySqlRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public AlbumController(IMySqlRepository repository, UserManager<ApplicationUser> userManager, IHostingEnvironment IHostingEnvironment)
        {
            _repository = repository;
            _userManager = userManager;
            _environment = IHostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var modelUser = _repository.GetUser(user.Id);
            List<Album> albums =await _repository.GetMyAlbumsAsync(user.Id);

            Models.AlbumViewModels.IndexViewModel model = new IndexViewModel();
            foreach (Album album in albums)
            {
                AlbumViewModel viewModel = new AlbumViewModel(album.Id, album.DateCreated, modelUser, album.Name);
                List<Photo> photos = await _repository.GetPhotosAsync(album.Id);
                if (photos.Count > 0)
                {
                    viewModel.ThumbnailImage = photos.First().ThumbnailImage;
                }
                model.Albums.Add(viewModel);
                
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

        public async Task<IActionResult> ShowAlbumPhotos(Guid id)
        {

            Album album = _repository.GetAlbum(id);
            AlbumViewModel model = new AlbumViewModel(album.Id, album.DateCreated, album.Owner, album.Name);
            model.Photos = await _repository.GetPhotosAsync(id);
            return View("Album", model);
        }

        [HttpGet]
        public IActionResult AddPhoto(Guid id)
        {
            Album album = _repository.GetAlbum(id);
            AlbumViewModel model=new AlbumViewModel(album.Id, album.DateCreated, album.Owner, album.Name);
            return View("AddPhoto",model);
        }

        //[HttpPost]
        public async Task<IActionResult> AddPhoto(String name, AlbumViewModel model)
        {
            var newFileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var modelUser = _repository.GetUser(user.Id);

                var fileName = string.Empty;
                string PathDB = string.Empty;

                var files = HttpContext.Request.Form.Files;

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Getting FileName
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Replace('"',' ').Trim();

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var FileExtension = Path.GetExtension(fileName);

                        // concating  FileName + FileExtension
                        newFileName = myUniqueFileName + FileExtension;

                        // Combines two strings into a path.
                        fileName = Path.Combine(_environment.WebRootPath, "uploads") + $@"\{newFileName}";
                        string thumbnail = Path.Combine(_environment.WebRootPath, "uploads") + $@"\thumbs" +
                                           $@"\{newFileName}";
                        // if you want to store path of folder in database
                        PathDB = "~/uploads/" + newFileName;
                        string thumbnailDB = "~/uploads/thumbs/" + newFileName;
                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }

                        CreateThumbnail(150, fileName, thumbnail);
                        await _repository.AddPhotoToAlbumAsync(model.Id, user.Id, PathDB, thumbnailDB);
                    }
                }
                RedirectToAction("ShowAlbumPhotos", model.Id); //redirect ne funkcionira kao zamisljeno
            }
            return View("AddPhoto",model);

        }

        void CreateThumbnail(int ThumbnailMax, string OriginalImagePath, string ThumbnailImagePath)
        {
            // Loads original image from file
            Image imgOriginal = Image.FromFile(OriginalImagePath);
            // Finds height and width of original image
            float OriginalHeight = imgOriginal.Height;
            float OriginalWidth = imgOriginal.Width;
            // Finds height and width of resized image
            int ThumbnailWidth;
            int ThumbnailHeight;
            if (OriginalHeight > OriginalWidth)
            {
                ThumbnailHeight = ThumbnailMax;
                ThumbnailWidth = (int)((OriginalWidth / OriginalHeight) * (float)ThumbnailMax);
            }
            else
            {
                ThumbnailWidth = ThumbnailMax;//popraviti da ne bude kvadrat?
                ThumbnailHeight = (int)((OriginalHeight / OriginalWidth) * (float)ThumbnailMax);
            }
            // Create new bitmap that will be used for thumbnail
            Bitmap ThumbnailBitmap = new Bitmap(ThumbnailWidth, ThumbnailHeight);
            Graphics ResizedImage = Graphics.FromImage(ThumbnailBitmap);
            // Resized image will have best possible quality
            ResizedImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
            ResizedImage.CompositingQuality = CompositingQuality.HighQuality;
            ResizedImage.SmoothingMode = SmoothingMode.HighQuality;
            // Draw resized image
            ResizedImage.DrawImage(imgOriginal, 0, 0, ThumbnailWidth, ThumbnailHeight);
            // Save thumbnail to file
            ThumbnailBitmap.Save(ThumbnailImagePath);
        }
    }

}