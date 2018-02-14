using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class User
    {
        public string Id { get; set; }

        public List<Album> Albums { get; set; }

        public List<User> Subscribed { get; set; }

        public List<Photo> FavotirePhotos { get; set; }

        public User(string Id)
        {
            this.Id = Id;
            Albums=new List<Album>();
            Subscribed=new List<User>();
            FavotirePhotos=new List<Photo>();
        }

        //for EF
        public User()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is User && obj != null)
            {
                var item = obj as User;
                return Id == item.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // TODO
        // public string ProfilePictureUrl { get; set; }
    }
}
