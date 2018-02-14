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
        public User Owner { get; set; }
        public string Name { get; set; }
        public List<Photo> Photos {get; set; }

        public Album(User Owner, string Name)
        {
            Id=Guid.NewGuid();
            DateCreated=DateTime.UtcNow;
            this.Owner = Owner;
            this.Name = Name;
            Photos=new List<Photo>();
        }

        //for EF
        public Album()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is Album && obj != null)
            {
                var item = obj as Album;
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
