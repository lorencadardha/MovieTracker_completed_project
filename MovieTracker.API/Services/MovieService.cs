using Microsoft.EntityFrameworkCore;
using MovieTracker.API.Data;
using MovieTracker.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTracker.API.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MediaItem> AddAsync(MediaItem movie)
        {
            _context.MediaItems.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<IEnumerable<MediaItem>> GetAllAsync()
        {
            return await _context.MediaItems.ToListAsync();
        }

        public async Task<MediaItem?> GetByIdAsync(long id)
        {
            return await _context.MediaItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MediaItem?> UpdateAsync(long id, MediaItem movie)
        {
            var existing = await _context.MediaItems.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return null;

            existing.Type = movie.Type;
            existing.Title = movie.Title;
            existing.Genre = movie.Genre;
            existing.Status = movie.Status;
            existing.Rating = movie.Rating;
            existing.Date = movie.Date;
            existing.Notes = movie.Notes;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await _context.MediaItems.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return;

            _context.MediaItems.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
