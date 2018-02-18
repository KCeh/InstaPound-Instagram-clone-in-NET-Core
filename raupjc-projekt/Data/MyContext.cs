using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace raupjc_projekt.Models
{
    public class MyContext: System.Data.Entity.DbContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<Album> Albums { get; set; }
        public IDbSet<Photo> Photos { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<LastPhoto> LastPhoto { get; set; }

        public MyContext(string cnnstr) : base(cnnstr)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LastPhoto>().HasKey(u => u.Id);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired();

            modelBuilder.Entity<Album>().HasKey(a => a.Id);
            modelBuilder.Entity<Album>().Property(a => a.Name).IsRequired();;
            modelBuilder.Entity<Album>().Property(a => a.DateCreated).IsRequired();

            modelBuilder.Entity<Photo>().HasKey(p => p.Id);
            modelBuilder.Entity<Photo>().Property(p => p.URL).IsRequired();
            modelBuilder.Entity<Photo>().Property(p => p.ThumbnailImage).IsRequired();
            modelBuilder.Entity<Photo>().Property(p => p.DateCreated).IsRequired();
            modelBuilder.Entity<Photo>().Property(p => p.HasComment).IsRequired();
            modelBuilder.Entity<Photo>().Property(p => p.NumberOfLikes).IsRequired();
            modelBuilder.Entity<Photo>().Property(p => p.Featured).IsRequired();

            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(c => c.Text).IsRequired();
            modelBuilder.Entity<Comment>().Property(c => c.DateCreated).IsRequired();

            modelBuilder.Entity<User>().HasMany(u => u.Albums).WithRequired(a => a.Owner);
           // modelBuilder.Entity<User>().HasMany(u => u.LikedPhotos).WithMany(p => p.UsersThatLiked);
            modelBuilder.Entity<User>().HasMany(u => u.Subscribed).WithMany(u => u.Subscribers);
           // modelBuilder.Entity<User>().HasMany(u => u.Comments).WithRequired(c => c.Commentator);
            modelBuilder.Entity<Album>().HasMany(u => u.Photos).WithRequired(a => a.Album);
            modelBuilder.Entity<Photo>().HasMany(u => u.Comments).WithRequired(a =>a.Photo);
        }
    }
}
