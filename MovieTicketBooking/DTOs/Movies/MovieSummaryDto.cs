namespace MovieTicketBooking.DTOs.Movies
{
    /// <summary>Used in paginated list responses — a lighter version without full description.</summary>
    public class MovieSummaryDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Rating { get; set; }
        public bool IsActive { get; set; }
    }
}
