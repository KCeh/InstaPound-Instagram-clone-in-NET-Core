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
        public int NumberOfLikes { get; set; }
        //TODO prilagoditi za kontroler
        public Photo(string URL)
        {
            Id = Guid.NewGuid();
            DateCreated=DateTime.UtcNow;
            this.URL = URL;
            Comments=new List<Comment>();
            NumberOfLikes = 0;
        }


    }
}
