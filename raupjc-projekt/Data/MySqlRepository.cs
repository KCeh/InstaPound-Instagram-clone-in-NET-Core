using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public User GetUser(string userId)
        {
            return _context.Users.Find(userId);
        }

        public List<Album> GetMyAlbumsAsync(string userId)
        {
           return _context.Users.Find(userId).Albums;
        }

        public async Task AddMyAlbumAsync(string ownerId, string name)
        {
            Album album = new Album(GetUser(ownerId),name);
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
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

        public List<Album> GetAllAlbums(string userId)
        {
            return _context.Albums.Where(a=>!a.Owner.Id.Equals(userId)).ToList();
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

        public Task SubscribeToUserAsync(string subscriberId, string ownerId)
        {
            throw new NotImplementedException();
        }

        public Task GetCommentsAsync(Guid photoId)
        {
            throw new NotImplementedException();
        }

        public Task PostCommentAsync(Guid photoId, string commentatorId, string text)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> GetFeaturedPhotosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> GetPhotosFromSubscribedUsersAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task FeaturePhotoAsync(Guid photoId)
        {
            throw new NotImplementedException();
        }
    }

    public class AccessDeniedException : Exception
    {

    }
}
