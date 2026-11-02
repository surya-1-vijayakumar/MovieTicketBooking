namespace MovieTicketBooking.Models
{
    /// <summary>
    /// Represents a screen/hall inside a theatre.
    /// </summary>
    public class Screen
    {
        public int ScreenId { get; set; }
        public int TheatreId { get; set; }
        public string ScreenName { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public string ScreenType { get; set; } = "Regular"; // Regular, IMAX, 4DX
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Theatre Theatre { get; set; } = null!;
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
