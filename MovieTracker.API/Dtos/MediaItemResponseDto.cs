namespace MovieTracker.API.Dtos;

public class MediaItemResponseDto
{
    public long Id { get; set; }
    public string Type { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int? Rating { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
}
