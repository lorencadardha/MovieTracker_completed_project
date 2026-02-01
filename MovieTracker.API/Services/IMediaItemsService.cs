using MovieTracker.API.Models;

namespace MovieTracker.API.Services;

public interface IMediaItemsService
{
    Task<List<MediaItem>> GetAllAsync();
    Task<MediaItem?> GetByIdAsync(long id);
    Task<MediaItem> AddAsync(MediaItem item);
    Task<MediaItem?> UpdateAsync(MediaItem item);
    Task<MediaItem?> UpdateStatusAsync(long id, string status);
    Task<bool> DeleteAsync(long id);
}
