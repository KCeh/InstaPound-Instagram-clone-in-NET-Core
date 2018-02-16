using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public interface IMySqlRepository
    {
        Task AddUserAsync(User user);
        User GetUser(string ownerId);
        Task<User> GetOwnerAsync(Guid albumId);
        Task<List<Album>> GetMyAlbumsAsync(string userId);
        Task AddMyAlbumAsync(Album album);
        Task<bool> RemoveMyAlbumAsync(User owner, Guid id);
        Task<bool> UpdateMyAlbumAsync(User owner, Album album);
        Task<Album> GetAlbumAsync(Guid albumId);

        Task<List<Photo>> GetPhotosAsync(Guid albumId);
        Task AddPhotoToAlbumAsync(Guid albumId, string ownerId, string url, string thumbnail);
        Task<bool> RemovePhotoFromAlbumAsync(Guid albumId, Guid photoId, string ownerId);

        Task<List<Album>> GetAllAlbumsAsync(string userId);
        Task FavoritePhotoAsync(string userId, Guid photoId);
        Task LikePhotoAsync(string userId, Guid photoId);//provjeri za bazu ako je ok

        List<Photo> GetFavoritePhotos(string userId);

        List<User> GetSubscribedUsers(string userId);
        Task SubscribeToUserAsync(string subscriberId, string ownerId);

        List<Comment> GetComments(Guid photoId);
        Task PostCommentAsync(Guid photoId, string commentatorId, string text);

        Task<List<Photo>> GetFeaturedPhotosAsync();
        List<Photo> GetPhotosFromSubscribedUsers(string userId);

        Task FeaturePhotoAsync(Guid photoId);


        
    }
}
