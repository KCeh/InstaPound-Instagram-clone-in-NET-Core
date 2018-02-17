using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using raupjc_projekt.Models.FavoriteViewModels;

namespace raupjc_projekt.Models
{
    public class IndexViewModelHome
    {
        public List<PhotoFavViewModel> Photos { get; set; }

        public IndexViewModelHome()
        {
            Photos = new List<PhotoFavViewModel>();
        }
    }
}
