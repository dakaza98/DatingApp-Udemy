using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PhotoRepository : IPhotoRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PhotoRepository(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Photo> GetPhotoById(int photoId)
    {
        return await _context.Photos
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(photo => photo.Id == photoId);
    }

    public async Task<List<PhotoForApprovalDTO>> GetUnapprovedPhotos()
    {
        var query = _context.Photos.IgnoreQueryFilters().Where(photo => photo.IsApproved == false);
        return await query
            .ProjectTo<PhotoForApprovalDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public void RemovePhoto(Photo photo)
    {
        _context.Photos.Remove(photo);
    }
}
