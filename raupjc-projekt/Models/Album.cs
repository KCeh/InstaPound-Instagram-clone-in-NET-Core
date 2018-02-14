using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class Album
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public List<Photo> Photos {get; set; }

        public Album(string OwnerId, string Name)
        {
            Id=Guid.NewGuid();
            DateCreated=DateTime.UtcNow;
            this.OwnerId = OwnerId;
            this.Name = Name;
            Photos=new List<Photo>();
        }
    }
}
