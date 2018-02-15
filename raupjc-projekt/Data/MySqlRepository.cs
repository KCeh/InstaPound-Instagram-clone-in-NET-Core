﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public class MySqlRepository:IMySqlRepository
    {
        private readonly MyContext _context;

        public MySqlRepository(MyContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public User GetUser(string userId)
        {
            return _context.Users.Find(userId);
        }

        public Task<List<Album>> GetMyAlbumsAsync(string userId)
        {
            return _context.Albums.Where(a => a.Owner.Id.Equals(userId)).ToListAsync(); //promijeniti i za ostalo
        }

        public async Task AddMyAlbumAsync(Album album)
        {
            try
            {
                _context.Albums.Add(album);
                await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            
        }

        public async Task<bool> RemoveMyAlbumAsync(string ownerId, Guid id)
        {
            var toRemove = _context.Albums.Find(id);
            if (!toRemove.Owner.Id.Equals(ownerId))
                throw new AccessDeniedException();
            _context.Albums.Remove(toRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMyAlbumAsync(string ownerId, Album album)
        {
            if (album != null)
            {
                if (!album.Owner.Id.Equals(ownerId))
                    throw new AccessDeniedException();
                _context.Entry(album).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            return false; //pogledati jos
        }

        public Album GeAlbum(Guid albumId)
        {
            return _context.Albums.Find(albumId);
        }

        public List<Photo> GetPhotos(Guid albumId)
        {
            return _context.Albums.Find(albumId).Photos;
        }

        public async Task AddPhotoToAlbumAsync(Guid albumId, string ownerId, string url)
        {
            Album album = GeAlbum(albumId);
            List<Photo> photos = GetPhotos(albumId);
            Photo photo=new Photo(url,album);
            photos.Add(photo);
            _context.Photos.Add(photo);
            _context.Entry(album).State = EntityState.Modified;//ok?? ili album ili photos
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemovePhotoFromAlbumAsync(Guid albumId, Guid photoId, string ownerId)
        {
            Album album = GeAlbum(albumId);
            Photo photo = _context.Photos.Find(photoId);
            if (album == null || photo == null)
                return false;
            _context.Photos.Remove(photo);
            album.Photos.Remove(photo);//radi???
            _context.Entry(album).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Album>> GetAllAlbumsAsync(string userId)
        {
            return await _context.Albums.Where(a=>!a.Owner.Id.Equals(userId)).ToListAsync();
        }

        public async Task FavoritePhotoAsync(string userId, Guid photoId)
        {
            Photo photo = _context.Photos.Find(photoId);
            User user = GetUser(userId);
            if (!user.FavotirePhotos.Contains(photo))
            {
                user.FavotirePhotos.Add(photo);
            }
            else
            {
                user.FavotirePhotos.Remove(photo);
            }
            _context.Entry(user).State= EntityState.Modified;//dobro?
            await _context.SaveChangesAsync();
        }

        public async Task LikePhotoAsync(string userId, Guid photoId)
        {
            Photo photo = _context.Photos.Find(photoId);
            User user = GetUser(userId);
            if (!user.LikedPhotos.Contains(photo))
            {
                photo.NumberOfLikes++;
                user.LikedPhotos.Add(photo);
            }
            else
            {
                photo.NumberOfLikes--;
                user.LikedPhotos.Remove(photo);
            }
            _context.Entry(user).State = EntityState.Modified;//dobro?
            _context.Entry(photo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public List<Photo> GetFavoritePhotos(string userId)
        {
            User user = GetUser(userId);
            return user.FavotirePhotos;
        }

        public List<User> GetSubscribedUsers(string userId)
        {
            User user = GetUser(userId);
            return user.Subscribed;
        }

        public async Task SubscribeToUserAsync(string subscriberId, string ownerId)
        {
            User subscriber = GetUser(subscriberId);
            User owner = GetUser(ownerId);
            subscriber.Subscribed.Add(owner);
            owner.Subscribers.Add(subscriber);
            _context.Entry(subscriber).State = EntityState.Modified;
            _context.Entry(owner).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public List<Comment> GetComments(Guid photoId)
        {
            Photo photo = _context.Photos.Find(photoId);
            return photo.Comments;
        }

        public async Task PostCommentAsync(Guid photoId, string commentatorId, string text)
        {
            Photo photo = _context.Photos.Find(photoId);
            Comment comment = new Comment(commentatorId, text, photo);
            photo.Comments.Add(comment);
            _context.Comments.Add(comment);
            _context.Entry(photo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Photo>> GetFeaturedPhotosAsync()
        {
            return await _context.Photos.Where(p => p.featured).ToListAsync();
        }

        public List<Photo> GetPhotosFromSubscribedUsers(string userId)
        {
            User user = GetUser(userId);
            List<Photo> photos = new List<Photo>();
            foreach (User u in user.Subscribed)
            {
                foreach (Album a in u.Albums)
                {
                    photos.AddRange(a.Photos);
                }
            }
            List<Photo> orderByDescending = photos.OrderByDescending(p => p.DateCreated).ToList();
            return orderByDescending;
        }

        public async Task FeaturePhotoAsync(Guid photoId)
        {
            //samo admin smije
            Photo photo = _context.Photos.Find(photoId);
            photo.featured = true;
            _context.Entry(photo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

    public class AccessDeniedException : Exception
    {

    }
}
