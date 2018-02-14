using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CommentatorId { get; set; }
        public string Text { get; set; }

        public Comment(string CommentatorId, string Text)
        {
            Id = Guid.NewGuid();
            DateCreated=DateTime.UtcNow;
            this.CommentatorId = CommentatorId;
            this.Text = Text;
        }
    }
}
