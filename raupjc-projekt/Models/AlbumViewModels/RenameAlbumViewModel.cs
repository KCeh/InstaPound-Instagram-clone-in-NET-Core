using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models.AlbumViewModels
{  
    public class RenameAlbumViewModel
    {
        public Guid Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }


        public RenameAlbumViewModel() { }

        public RenameAlbumViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
