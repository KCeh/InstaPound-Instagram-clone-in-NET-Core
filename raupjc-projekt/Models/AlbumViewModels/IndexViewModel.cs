﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models.AlbumViewModels
{
    public class IndexViewModel
    {
        public List<AlbumViewModel> Albums { get; set; }

        public IndexViewModel()
        {
            Albums=new List<AlbumViewModel>();
        }
    }
}
