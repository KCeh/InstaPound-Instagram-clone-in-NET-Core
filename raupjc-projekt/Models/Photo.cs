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
        public List<Comment> Comments { get; set; }
        public List<User> usersThatLiked { get; set; }
        public int NumberOfLikes { get; set; }
        public Album Album { get; set; }
        public List<User> UserFavorited { get; set; }
        public bool featured { get; set; }

        public bool HasComment
        {
            get { return !Comments.Any(); }
            set { }
        }

        //TODO prilagoditi za kontroler
        public Photo(string URL, Album album)
        {
            Id = Guid.NewGuid();
            DateCreated=DateTime.UtcNow;
            this.URL = URL;
            Comments=new List<Comment>();
            usersThatLiked=new List<User>();
            NumberOfLikes = 0;
            this.Album = album;
            UserFavorited=new List<User>();
            featured = false;
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
