﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public User Commentator { get; set; }
        public string Text { get; set; }
        public Photo Photo { get; set; }

        public Comment(User Commentator, string Text, Photo photo)
        {
            Id = Guid.NewGuid();
            DateCreated=DateTime.UtcNow;
            this.Commentator = Commentator;
            this.Text = Text;
            this.Photo = photo;
        }

        //for EF
        public Comment()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is Comment && obj != null)
            {
                var item = obj as Comment;
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
