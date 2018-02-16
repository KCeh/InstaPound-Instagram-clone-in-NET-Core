using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models.FavoriteViewModels
{
    public class FavoriteViewModel
    {
        public List<PhotoFavViewModel> Photos { get; set; }

        public FavoriteViewModel()
        {
            Photos=new List<PhotoFavViewModel>();
        }
    }

}
