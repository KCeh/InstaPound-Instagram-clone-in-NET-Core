using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models.AlbumViewModels
{
    public class AddAlbumViewModel
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        public AddAlbumViewModel(){}

        public AddAlbumViewModel(string Name)
        {
            this.Name = Name;
        }

    }
}
