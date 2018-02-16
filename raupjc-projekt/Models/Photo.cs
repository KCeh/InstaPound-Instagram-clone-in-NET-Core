using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class Photo
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string URL { get; set; }
        public string ThumbnailImage { get; set; }
        public List<Comment> Comments { get; set; }
        //public List<User> UsersThatLiked { get; set; }
        public int NumberOfLikes { get; set; }
        public Album Album { get; set; }
        public bool featured { get; set; }

        public bool HasComment
        {
            get
            {
                if (Comments == null) return false;
                return Comments.Any();
            }
            set { }
        }

        //TODO prilagoditi za kontroler
        public Photo(string URL, Album album, string thumbnail)
        {
            Id = Guid.NewGuid();
            DateCreated=DateTime.UtcNow;
            this.URL = URL;
            Comments=new List<Comment>();
            //UsersThatLiked=new List<User>();
            NumberOfLikes = 0;
            this.Album = album;
            featured = false;
            ThumbnailImage = thumbnail;
        }

        //for EF
        public Photo()
        {
            
        }

        public override bool Equals(object obj)
        {
            if (obj is Photo && obj != null)
            {
                var item = obj as Photo;
                return Id == item.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
