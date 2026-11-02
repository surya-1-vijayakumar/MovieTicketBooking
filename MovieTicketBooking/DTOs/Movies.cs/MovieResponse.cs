
namespace MovieTicketBooking.DTOs.Movies
{
    public class MovieResponse
    {
        public int MovieId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DurationMinutes { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Language { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;
    }
}
