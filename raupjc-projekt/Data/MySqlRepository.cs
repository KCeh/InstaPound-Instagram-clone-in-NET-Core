using System;
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
            return _context.Users.FirstOrDefault(u => u.Id.Equals(userId));
        }

        public async Task<User> GetOwnerAsync(Guid albumId)
        {
            Album album =await GetAlbumAsync(albumId);
            return album.Owner;
        }

        public Task<List<Album>> GetMyAlbumsAsync(string userId)
        {
            return _context.Albums.Where(a => a.Owner.Id.Equals(userId)).Include(a=>a.Owner).ToListAsync();
        }

        public async Task AddMyAlbumAsync(Album album)
        {
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveMyAlbumAsync(User owner, Guid id)
        {
            var toRemove = _context.Albums.Find(id);
            if (!toRemove.Owner.Equals(owner))
                throw new AccessDeniedException();
            _context.Albums.Remove(toRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMyAlbumAsync(User owner, Album album)
        {
            if (album != null)
            {
                if (!album.Owner.Equals(owner))
                    throw new AccessDeniedException();
                _context.Entry(album).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Album> GetAlbumAsync(Guid albumId)
        {

            return await _context.Albums.Where(a => a.Id.Equals(albumId)).Include(a=>a.Owner).FirstOrDefaultAsync();
        }

        public async Task<List<Photo>> GetPhotosAsync(Guid albumId)
        {
            return await _context.Photos.Where(p => p.Album.Id.Equals(albumId)).ToListAsync();
        }

        public async Task AddPhotoToAlbumAsync(Guid albumId, string ownerId, string url, string thumbnail)
        {
            Album album = await GetAlbumAsync(albumId);
            List<Photo> photos = await GetPhotosAsync(albumId);
            Photo photo=new Photo(url,album, thumbnail);
            photos.Add(photo);
            _context.Photos.Add(photo);
            _context.Entry(album).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemovePhotoFromAlbumAsync(Guid albumId, Guid photoId, string ownerId)
        {
            Album album = await GetAlbumAsync(albumId);
            Photo photo = _context.Photos.Find(photoId);
            if (album == null || photo == null)
                return false;
            List<Photo> photos = await GetPhotosAsync(albumId);
            photos.Remove(photo);
            _context.Photos.Remove(photo);
            _context.Entry(album).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Album>> GetAllAlbumsAsync(string userId)
        {
            return await _context.Albums.Where(a=>!a.Owner.Id.Equals(userId)).Include(a=>a.Owner).ToListAsync();
        }

        public async Task FavoritePhotoAsync(string userId, Guid photoId)
        {
            Photo photo = await GetPhotoAsync(photoId);
            User user = await GetUserWithAsync(userId);
            if (!user.FavotirePhotos.Contains(photo))
            {
                user.FavotirePhotos.Add(photo);
            }
            else
            {
                user.FavotirePhotos.Remove(photo);
            }
            _context.Entry(user).State= EntityState.Modified;
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

        public async Task<List<Photo>> GetFavoritePhotos(string userId)
        {
            User user = await GetUserWithAsync(userId);
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

        public async Task PostCommentAsync(Guid photoId, User commentator, string text)
        {
            Photo photo = _context.Photos.Find(photoId);
            Comment comment = new Comment(commentator, text, photo);
            photo.Comments.Add(comment);
            _context.Comments.Add(comment);
            _context.Entry(photo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Photo>> GetFeaturedPhotosAsync()
        {
            return await _context.Photos.Where(p => p.featured).Include(p=>p.Album).ToListAsync();
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

        private async Task<User> GetUserWithAsync(string userId)
        {
            return await _context.Users.Where(u => u.Id.Equals(userId)).Include(u => u.FavotirePhotos)
                .Include(u => u.LikedPhotos).FirstOrDefaultAsync();
        }

        public async Task FeaturePhotoAsync(Guid photoId)
        {
            //samo admin smije
            Photo photo = _context.Photos.Find(photoId);
            photo.featured = true;
            _context.Entry(photo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public Task<Photo> GetPhotoAsync(Guid photoId)
        {
            return _context.Photos.Where(p => p.Id.Equals(photoId))
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserId(Guid photoId)
        {
            Photo photo = await _context.Photos.Where(p => p.Id.Equals(photoId))
                .Include(p => p.Album).FirstOrDefaultAsync();
            Album album = await _context.Albums.Where(a => a.Id.Equals(photo.Album.Id)).Include(a => a.Owner)
                .FirstOrDefaultAsync();
            return album.Owner;
        }
    }

    public class AccessDeniedException : Exception
    {

    }
}
