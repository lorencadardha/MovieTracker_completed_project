

namespace MovieTracker.API.Dtos
{
    public class MediaItemRequestDto
    {
        public string Type { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? Rating { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}


