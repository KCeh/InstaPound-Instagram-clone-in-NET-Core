using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raupjc_projekt.Models
{
    public interface IMySqlRepository
    {
        Task<List<Album>> getMyAlbumsAsync(string userId);
        Task addMyAlbumAsync(string ownerId, string name);
        Task<bool> removeMyAlbumAsync(string ownerId, Guid Id);
        Task<bool> updateMyAlbumAsync(string ownerId, Guid Id);

        Task<List<Photo>> getPhotosAsync(Guid albumId);
        Task addPhotoToAlbumAsync(Guid albumId, string ownerId, string URL);
        Task<bool> removePhotoFromAlbumAsync(Guid albumId, Guid photoId, string ownerId);

        Task getAllAlbumsAsync();
        Task favoritePhotoAsync(string userId, Guid photoId);
        Task likePhotoAsync(string GuidPhotoId);//provjeri za bazu ako je ok

        Task<List<Photo>> getFavoritePhotosAsync(string userId);

        Task<List<User>> getSubscribedUsersAsync(string userId);
        Task subscribeToUserAsync(string subscriberId, string ownerId);

        Task getCommentsAsync(Guid photoId);
        Task postCommentAsync(Guid photoId, string commentatorId, string text);

        Task<List<Photo>> getFeaturedPhotosAsync();
        Task<List<Photo>> getPhotosFromSubscribedUsersAsync(string userId);

        Task featurePhotoAsync(Guid photoId);


    }
}
