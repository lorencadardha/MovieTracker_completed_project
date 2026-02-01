using System.ComponentModel.DataAnnotations;

namespace MovieTracker.API.Models
{
    public class MediaItem
    {
        public long Id { get; set; }

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Genre { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!;

        public int? Rating { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? Notes { get; set; }
    }
}
