using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class FeatureViewModel
    {
        public List<Photo> Photos { get; set; }

        public FeatureViewModel()
        {
            Photos = new List<Photo>();
        }
    }
}
