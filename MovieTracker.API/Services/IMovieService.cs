using MovieTracker.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTracker.API.Services
{
    public interface IMovieService
    {
        Task<MediaItem> AddAsync(MediaItem movie);
        Task<MediaItem?> GetByIdAsync(long id);
        Task<IEnumerable<MediaItem>> GetAllAsync();
        Task<MediaItem?> UpdateAsync(long id, MediaItem movie);
        Task DeleteAsync(long id);
    }
}
