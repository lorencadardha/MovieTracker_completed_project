using Microsoft.EntityFrameworkCore;
using MovieTracker.API.Data;
using MovieTracker.API.Models;

namespace MovieTracker.API.Services;

public class MediaItemsService(AppDbContext db) : IMediaItemsService
{
    public async Task<List<MediaItem>> GetAllAsync()
        => await db.MediaItems.OrderByDescending(x => x.Date).ToListAsync();

    public async Task<MediaItem?> GetByIdAsync(long id)
        => await db.MediaItems.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<MediaItem> AddAsync(MediaItem item)
    {
        await db.MediaItems.AddAsync(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<MediaItem?> UpdateAsync(MediaItem item)
    {
        var existing = await db.MediaItems.FirstOrDefaultAsync(x => x.Id == item.Id);
        if (existing == null) return null;

        existing.Type = item.Type;
        existing.Title = item.Title;
        existing.Genre = item.Genre;
        existing.Status = item.Status;
        existing.Rating = item.Rating;
        existing.Date = item.Date;
        existing.Notes = item.Notes;

        db.MediaItems.Update(existing);
        await db.SaveChangesAsync();
        return existing;
    }

    public async Task<MediaItem?> UpdateStatusAsync(long id, string status)
    {
        var existing = await db.MediaItems.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null) return null;

        existing.Status = status;

        // Simple rule: if not watched, remove rating
        if (!string.Equals(status, "watched", StringComparison.OrdinalIgnoreCase))
            existing.Rating = null;

        db.MediaItems.Update(existing);
        await db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existing = await db.MediaItems.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null) return false;

        db.MediaItems.Remove(existing);
        await db.SaveChangesAsync();
        return true;
    }
}
