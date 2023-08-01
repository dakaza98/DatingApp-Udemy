using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IPhotoRepository
{
    public Task<List<PhotoForApprovalDTO>> GetUnapprovedPhotos();
    public Task<Photo> GetPhotoById(int photoId);
    public void RemovePhoto(Photo photo);
}
