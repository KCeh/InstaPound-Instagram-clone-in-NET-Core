using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class CommentsViewModel
    {
        public List<Comment> Comments { get; set; }
        public string Text { get; set; }
        public Guid Photo { get; set; }

        public CommentsViewModel()
        {
            Comments=new List<Comment>();
            Text = "";
        }
        public CommentsViewModel(Guid photo)
        {
            Comments = new List<Comment>();
            Text = "";
            Photo = photo;
        }
    }
}
