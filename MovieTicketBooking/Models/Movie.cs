namespace MovieTicketBooking.Models
{
    /// <summary>
    /// Represents a movie available for booking.
    /// </summary>
    public class Movie
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
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property — one movie has many shows
        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
