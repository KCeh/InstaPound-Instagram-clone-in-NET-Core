using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public interface IMySqlRepository
    {
        User GetUser(string ownerId);
        List<Album> GetMyAlbumsAsync(string userId);
        Task AddMyAlbumAsync(string ownerId, string name);
        Task<bool> RemoveMyAlbumAsync(string ownerId, Guid id);
        Task<bool> UpdateMyAlbumAsync(string ownerId, Album album);
        Album GeAlbum(Guid albumId);

        List<Photo> GetPhotos(Guid albumId);
        Task AddPhotoToAlbumAsync(Guid albumId, string ownerId, string url);
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
