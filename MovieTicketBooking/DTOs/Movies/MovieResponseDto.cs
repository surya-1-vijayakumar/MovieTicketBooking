namespace MovieTicketBooking.DTOs.Movies
{
    public class MovieResponseDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Rating { get; set; }
        public string? PosterUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
