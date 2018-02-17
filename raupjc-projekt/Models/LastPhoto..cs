using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class LastPhoto
    {
        public Guid Id { get; set; }

        public LastPhoto()
        {
            
        }

        public LastPhoto(Guid id)
        {
            Id = id;
        }
    }
}
