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
        Task<List<Photo>> GetAllPhotosAsync();
        Task AddPhotoToAlbumAsync(Guid albumId, string ownerId, string url, string thumbnail);
        Task<bool> RemovePhotoFromAlbumAsync(Guid albumId, Guid photoId, string ownerId);

        Task<List<Album>> GetAllAlbumsAsync(string userId);
        Task FavoritePhotoAsync(string userId, Guid photoId);
        Task LikePhotoAsync(string userId, Guid photoId);//provjeri za bazu ako je ok

        Task<List<Photo>> GetFavoritePhotos(string userId);

        List<User> GetSubscribedUsers(string userId);
        Task SubscribeToUserAsync(string subscriberId, string ownerId);

        Task<List<Comment>> GetCommentsAsync(Guid photoId);
        Task PostCommentAsync(Guid photoId, User commentator, string text);

        Task<List<Photo>> GetFeaturedPhotosAsync();
        Task<List<Photo>> GetPhotosFromSubscribedUsersAsync(string userId);

        Task FeaturePhotoAsync(Guid photoId);

        Task<Photo> GetPhotoAsync(Guid photoId);
        Task<User> GetUserId(Guid photoId);
        LastPhoto GetLastCommentedPhoto();
        void SaveLastCommented(Guid id);
    }
}
