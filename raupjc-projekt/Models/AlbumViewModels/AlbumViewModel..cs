using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models.AlbumViewModels
{
    public class AlbumViewModel
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; }
        public string ThumbnailImage;

        public AlbumViewModel() { }

        public AlbumViewModel(Guid id, DateTime dateCreated, User owner, string name)
        {
            Id = id;
            DateCreated = dateCreated;
            Owner = owner;
            Name = name;
            ThumbnailImage = "~/images/placeholder.svg";
        }
    }
}
