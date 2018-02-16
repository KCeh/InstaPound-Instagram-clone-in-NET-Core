using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models.FavoriteViewModels
{
    public class PhotoFavViewModel
    {
        public Guid Id { get; set; }
        public string URL { get; set; }
        public User OriginalPoster { get; set; }
        public int NumberOfLikes { get; set; }

        public PhotoFavViewModel(Photo photo, User owner)
        {
            OriginalPoster = owner;
            Id = photo.Id;
            URL = photo.URL;
            NumberOfLikes = photo.NumberOfLikes;
        }
    }
}
